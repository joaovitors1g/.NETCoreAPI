using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.CrossCuting.DependencyInjection;
using Api.CrossCuting.Mappings;
using Api.Data.Context;
using Api.Domain.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

         var config = new MapperConfiguration(cfg => 
         {
            cfg.AddProfile(new DTOToModelProfile());
            cfg.AddProfile(new EntityToDTOProfile());
            cfg.AddProfile(new ModelToEntityProfile());
         });

         IMapper mapper = config.CreateMapper();

         services.AddSingleton(mapper);

         services.AddControllers();

         var signingConfigurations = new SigningConfiguration();
         services.AddSingleton(signingConfigurations);

         var tokenConfigurations = new TokenConfiguration();
         new ConfigureFromConfigurationOptions<TokenConfiguration>(
            Configuration.GetSection("TokenConfiguration")
         ).Configure(tokenConfigurations);

         services.AddSingleton(tokenConfigurations);

         services.AddAuthentication(authOptions => 
         {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(bearerOptions =>
         {
            var paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = tokenConfigurations.Audience;
            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
            paramsValidation.ValidateIssuerSigningKey = true;
            paramsValidation.ClockSkew = TimeSpan.Zero;
         });

         services.AddAuthorization(auth =>
         {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                        .RequireAuthenticatedUser().Build());
         });

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

             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                Description = "Insira o token JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
             });

             c.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                {
                  new OpenApiSecurityScheme
                  {
                     Reference = new OpenApiReference
                     {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                     }
                  },
                  new List<string>()
                }
             });
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

         if (Environment.GetEnvironmentVariable("EXECUTE_MIGRATIONS").ToLower() == "true")
         {
            using (var service = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                                                         .CreateScope())
            {
               using (var context = service.ServiceProvider.GetService<MyContext>())
               {
                  context.Database.Migrate();
               }
            }
         }
      }
   }
}
