using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using shtormteh.configuration.service.Config;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shtormteh.configuration.service
{
    public class Startup
    {
        const string BaseSectionConfig = "BaseConfiguration";
        public SwaggerConfig SwaggerConfig => Configuration.GetSection(BaseSectionConfig).Get<BaseConfiguration>().SwaggerConfig;
        private readonly IWebHostEnvironment _hostingEnv;
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            _hostingEnv = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddHealthChecks();
            services.AddOptions();
            if (SwaggerConfig.IsEnabled)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v0", new OpenApiInfo { Title = "shtormteh.configuration.service", Version = "v0" });
                    var xmlPath = $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml";
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (SwaggerConfig.IsEnabled)
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {                       
                        swaggerDoc.Servers = new List<OpenApiServer> {
                            new OpenApiServer { Url = $"http://{httpReq.Host.Value}/{SwaggerConfig.EndpointPrefix.Trim('/')}" },
                            new OpenApiServer { Url = $"https://{httpReq.Host.Value}/{SwaggerConfig.EndpointPrefix.Trim('/')}" }
                        };
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{SwaggerConfig.EndpointPrefix.TrimEnd('/')}/swagger/v0/swagger.json", "shtormteh.configuration.service API V0");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHealthChecks("/health");
        }
    }
}
