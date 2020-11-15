using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.CrossCuting.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace application
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
         ConfigureService.ConfigureDependenciesService(services);
         ConfigureRepository.ConfigureDependenciesRepository(services);
         services.AddControllers();
         services.AddSwaggerGen(
           c =>
           {
              c.SwaggerDoc("v1", new OpenApiInfo
              {
                 Version = "v1",
                 Title = "Curso API .NET Core 3.1",
                 Description = "Arquitetura DDD",
                 TermsOfService = new Uri("http://github.com/joaovitors1g"),
                 Contact = new OpenApiContact
                 {
                    Name = "João Vitor",
                    Email = "joao@email.com",
                    Url = new Uri("http://github.com/joaovitors1g")
                 },
                 License = new OpenApiLicense
                 {
                    Name = "Termos de Uso da API",
                    Url = new Uri("http://github.com/joaovitors1g")
                 }
              }
           );
           }
         );
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseSwagger();

         app.UseSwaggerUI(
           c =>
           {
              c.SwaggerEndpoint("/swagger/v1/swagger.json", "Curso API .NET Core 3.1");
              c.RoutePrefix = string.Empty;
           }
         );

         app.UseRouting();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}
