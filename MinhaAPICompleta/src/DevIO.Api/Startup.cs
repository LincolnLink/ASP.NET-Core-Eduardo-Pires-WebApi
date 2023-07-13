using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
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


        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(hostingEnvironment.ContentRootPath) //pasta local
                 .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile(path: $"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            if(hostingEnvironment.IsDevelopment()) //se for de dev trabalha com secrets
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
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

            //LIB paga não implementada.
            //services.AddLoggingConfiguration();


            //Codigo antigo
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});

            // lib paga, não implementada.
            //services.AddLoggingConfig(Configuration);

            //Não tem
            /*
            services.AddHealthChecks()
                .AddCheck("Produtos", new SqlServerHealthCheck(Configuration.GetConnectionString(name: "DefaultConnection")))
                .AddSqlServer(Configuration.GetConnectionString(name: "DefaultConnection"), name: "BancoSQL");
            services.AddHealthChecksUI();*/

            services.ResolveDependencies();

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {  
            app.UseApiConfig(env);

            app.UseSwaggerConfig(provider);

            //app.UseHealthChecks("/hc");

            //Arquivo que tem uma lib paga.
            //app.UseLoggingConfiguration();
        }
    }
}
