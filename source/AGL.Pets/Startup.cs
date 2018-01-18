using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AGL.Pets.Core.Domain.ViewModels;
using AGL.Pets.Core.Interfaces;
using AGL.Pets.Service;
using AGL.Pets.Service.Repositories;
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

            services.AddScoped<IPetsRepository, PetsRepository>();
            services.AddScoped<IPetsService, PetsService>();

            //Microsoft patterns and practices suggest not to use using block with HttpClient as it implements IDisposable indirectly.
            //Instead Microsoft suggests to use this anti pattern to share single instance of HttpClient.
            //https://docs.microsoft.com/en-us/azure/architecture/antipatterns/improper-instantiation/
            services.AddSingleton<HttpClient>();

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
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
