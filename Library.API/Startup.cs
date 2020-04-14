using AutoMapper;
using Library.API.Entities;
using Library.API.Filters;
using Library.API.Servicers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.ReturnHttpNotAcceptable = true;//ֻ�з���Acceptָ�����Ϳͻ��˲�����ȷ���գ����򷵻�406 Not Accept
                //config.OutputFormatters.Add(new XmlSerializerOutputFormatter());//���ܹ����XML��Formatter��ӵ�Formatter������
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
