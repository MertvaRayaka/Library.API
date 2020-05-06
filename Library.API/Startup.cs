using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Servicers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Library.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add<JsonExceptionFilter>();

                config.ReturnHttpNotAcceptable = true;//只有返回Accept指定类型客户端才能正确接收，否则返回406 Not Accept
                //config.OutputFormatters.Add(new XmlSerializerOutputFormatter());//将能够输出XML的Formatter添加到Formatter集合中

                config.CacheProfiles.Add("60", new CacheProfile()
                {
                    Duration = 60,
                    Location = ResponseCacheLocation.Client,
                });
            })
            .AddNewtonsoftJson();
            //services.AddScoped<IAuthorRepository, AuthorMockRepository>();
            //services.AddScoped<IBookRespository, BookMockRepository>();
            services.AddDbContext<LibraryDbContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("MysqlConnection"));
            });

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddAutoMapper(typeof(Library.API.Helpers.LibraryMappingProfile).Assembly);

            services.AddScoped<CheckAuthorExistFilterAttribute>();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
                options.MaximumBodySize = 1024;
            });

            services.AddMemoryCache();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Caching:Host"];
                options.InstanceName = Configuration["Caching:Instanse"];
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                //当客户端未提供版本时是否使用默认版本
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                //在requestHeader中为属性api-version赋值，并且可以在URL中使用查询语句
                options.ApiVersionReader = ApiVersionReader.Combine
                (
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("api-version")
                );
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
