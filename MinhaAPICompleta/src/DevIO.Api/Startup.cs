using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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

            // Passa o configuration porq vai usar a conectionString.
            services.AddIdentityConfig(Configuration);

            services.AddAutoMapper(typeof(Startup)); 

            services.AddApiConfig();
                    
            services.AddSwaggerConfig();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});

            //services.AddLoggingConfig(Configuration);

            services.ResolveDependencies();

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {  
            app.UseApiConfig(env);

            app.UseSwaggerConfig(provider);

            //app.UseLoggingConfiguration();
        }
    }
}
