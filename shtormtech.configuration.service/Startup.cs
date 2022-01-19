using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using shtormtech.configuration.git;
using shtormtech.configuration.service.Config;
using shtormtech.configuration.service.Services;

using System;
using System.Collections.Generic;
using System.IO;

namespace shtormtech.configuration.service
{
    public class Startup
    {
        const string BaseSectionConfig = "BaseConfiguration";
        const string HealthCheckUri = @"/health";
        private readonly IWebHostEnvironment _hostingEnv;
        public SwaggerConfig SwaggerConfig { get; }
        public GitConfig GitConfiguration { get; }
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
            var baseConfiguration = Configuration.GetSection(BaseSectionConfig).Get<BaseConfiguration>();
            SwaggerConfig = baseConfiguration.SwaggerConfig;
            GitConfiguration = baseConfiguration.Git;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICommands>(provider => new Commands(GitConfiguration.Uri, GitConfiguration.User, GitConfiguration.Password));
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRepositoryService, RepositoryService>();

            services.AddControllers();
            services.AddHealthChecks();
            services.AddOptions();
            services.Configure<BaseConfiguration>(Configuration.GetSection(BaseSectionConfig));
            if (SwaggerConfig.IsEnabled)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v0", new OpenApiInfo { Title = "shtormtech.configuration.service", Version = "v0" });
                    var xmlPath = $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml";
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }
        }

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
                    c.SwaggerEndpoint($"{SwaggerConfig.EndpointPrefix.TrimEnd('/')}/swagger/v0/swagger.json", "shtormtech.configuration.service API V0");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHealthChecks(HealthCheckUri);
        }
    }
}
