using Gtm.WebService.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// The main startup class for the WebApi.
   /// </summary>
   public class Startup
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Startup"/> class.
      /// </summary>
      /// <param name="env">The hosting environment.</param>
      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("PackageRoot/Config/appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"PackageRoot/Config/appsettings.{env.EnvironmentName}.json", optional: true)
             .AddApplicationInsightsSettings(env.IsDevelopment())
             .AddEnvironmentVariables();
         Configuration = builder.Build();
      }

      /// <summary>
      /// Gets the configuration.
      /// </summary>
      public IConfigurationRoot Configuration { get; }

      /// <summary>
      /// This method gets called by the runtime. Use this method to add services to the container.
      /// </summary>
      /// <param name="services">The list of services.</param>
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMvc(options =>
         {
            options.RespectBrowserAcceptHeader = true;
            options.OutputFormatters.Add(new PascalCaseJsonProfileFormatter());
         });

         services.AddTransient<IDesignService, DesignServiceProxy>();
         services.AddTransient<IPartnerService, PartnerServiceProxy>();

         services.AddSwaggerGen(options =>
         {
            var info = new Info
            {
               Version = "v1",
               Title = "Gtm.WebService API",
               Description = "Documentation for end-points and usage around the Gtm.WebService RESTful API Service.",
               Contact = new Contact
               {
                  Name = "GTM Web Development Team",
                  Email = "webteam@igtm.com",
                  Url = "https://gtmsportswear.com"
               },
               TermsOfService = "For use only by It's Greek To Me, Inc. or licensed partners."
            };

            options.IncludeXmlComments("Gtm.WebService.WebApi.xml");
            options.DocumentFilter<ControllerDescriptionsDocumentFilter>();
            options.SwaggerDoc("api", info);
            options.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Name = "Authorization", Description = "Please insert JWT with Bearer into field, such as \"bearer {token}\"", Type = "apiKey" });
         });

         services.AddAuthentication();
         services.AddAntiforgery();
         services.AddCors();
      }

      /// <summary>
      /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      /// </summary>
      /// <param name="app">The application builder class.</param>
      /// <param name="env">The hosting environment.</param>
      /// <param name="loggerFactory">The logger factory.</param>
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         loggerFactory.AddConsole(Configuration.GetSection("Logging"));
         loggerFactory.AddDebug();

         if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

         app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
         app.UseForwardedHeaders();
         app.UseCors(corsPolicy =>
         {
            corsPolicy.AllowAnyHeader();
            corsPolicy.AllowAnyMethod();
            corsPolicy.AllowAnyOrigin();
            corsPolicy.AllowCredentials();
         });

         app.UseMvc();

         app.UseSwagger();
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/api/swagger.json", "Gtm.WebService API Documentation");
            c.ShowRequestHeaders();
            c.ShowJsonEditor();
         });
      }
   }
}
