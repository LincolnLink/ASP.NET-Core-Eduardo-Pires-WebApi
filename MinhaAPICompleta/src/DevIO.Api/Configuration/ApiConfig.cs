using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public static class ApiConfig
    {

        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();

            /*
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });*/

            // Para personalizar a modelState.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Setup CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                   configurePolicy: builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                     );                
                
                options.AddPolicy(name: "Production",
                    configurePolicy: builder =>
                     builder
                     .WithMethods("GET")
                     .WithOrigins("http://localhost:4200", "https://localhost:4200")
                     .SetIsOriginAllowedToAllowWildcardSubdomains()
                     //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                     .AllowAnyHeader());
                    

            });

            //services.AddHealthChecksUI();

            return services;

        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseCors("AllowAll");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production"); // Usar apenas nas demos => Configuração Ideal: Production
                app.UseHsts();
            }

            //app.UseMiddleware<ExceptionMiddleware>();
            //app.UseHttpsRedirection();

           
            //app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
