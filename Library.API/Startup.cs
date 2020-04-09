using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Library.API.Servicers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Library.API.Entities;
using Microsoft.EntityFrameworkCore;

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
                config.ReturnHttpNotAcceptable = true;//只有返回Accept指定类型客户端才能正确接收，否则返回406 Not Accept
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());//将能够输出XML的Formatter添加到Formatter集合中
            })
            .AddNewtonsoftJson();
            services.AddScoped<IAuthorRepository, AuthorMockRepository>();
            services.AddScoped<IBookRespository, BookMockRepository>();
            services.AddDbContext<LibraryDbContext>(option => 
            {
                option.UseMySQL(Configuration.GetConnectionString("MysqlConnection"));
            });
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
