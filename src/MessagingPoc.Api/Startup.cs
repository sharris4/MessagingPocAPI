using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag;

namespace MessagingPoc.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));
            // Register the Swagger services
            services.AddSwaggerDocument(document =>
            {
                document.PostProcess = d =>
                {
                    d.Info.Version = "v1";
                    d.Info.Title = "Messaging POC API";
                    d.Info.Description = "Web API for Examples";
                    d.Info.TermsOfService = "None";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            // swagger enable
            bool swaggerEnable = configuration.GetSection("SwaggerEnable").Value == "true";
            bool reverseProxy = false;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseMvc();

            //Add Swagger - dotnet add ApiTemplate.csproj package NSwag.
            if (swaggerEnable)
            {
                app.UseSwagger(settings =>
                {
                    settings.PostProcess = (document, request) =>
                    {
                        document.Schemes.Clear();

                        if (reverseProxy)
                        {
                            document.Schemes.Add(SwaggerSchema.Https);
                        }
                        else
                        {
                            document.Schemes.Add(SwaggerSchema.Http);
                        }
                    };
                });
                app.UseSwaggerUi3(settings =>
                {
                    settings.TransformToExternalPath = (internalUiRoute, request) =>
                    {
                        if (internalUiRoute.StartsWith("/") == true && internalUiRoute.StartsWith(request.PathBase) == false)
                        {
                            return request.PathBase + internalUiRoute;
                        }
                        else
                        {
                            return internalUiRoute;
                        }
                    };
                });
            }
        }
    }
}
