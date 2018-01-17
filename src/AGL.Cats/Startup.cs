using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AGL.Cats.Core.Interfaces;
using AGL.Cats.Service;
using AGL.Cats.Service.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace AGL.Cats
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
            services.AddMvc();

            var entryAssembly = Assembly.GetEntryAssembly();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Version = "v1", Title = "AGL Cats API" });
                options.IncludeXmlComments(Path.ChangeExtension(entryAssembly.Location, "xml"));
            });

            services.AddScoped<ICatsRepository, CatsRepository>();
            services.AddScoped<ICatsService, CatsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "AGL Cats API v1"));
            app.UseMvcWithDefaultRoute();
        }
    }
}
