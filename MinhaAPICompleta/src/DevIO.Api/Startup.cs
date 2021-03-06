using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
