using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }        

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //services.AddIdentityConfig(Configuration);

            services.AddAutoMapper(typeof(Startup)); 

            services.AddApiConfig();

            //services.AddSwaggerConfig();

            //services.AddLoggingConfig(Configuration);

            services.ResolveDependencies();

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {  
            app.UseApiConfig(env);

            //app.UseSwaggerConfig(provider);

            //app.UseLoggingConfiguration();
        }
    }
}
