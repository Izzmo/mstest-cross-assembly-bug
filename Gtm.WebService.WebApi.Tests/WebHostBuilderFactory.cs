using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Gtm.WebService.WebApi.Tests
{
   public static class WebHostBuilderFactory
   {
      private static IHostingEnvironment hostingEnvironment;

      public static IWebHostBuilder Create()
      {
         return Create((Action<IServiceCollection>)null);
      }

      public static IWebHostBuilder Create(Action<IServiceCollection> configureServices)
      {
         var httpContexts = new ConcurrentQueue<HttpContext>();

         Action<IApplicationBuilder> captureHttpContext = builder => builder.Use(async (httpContext, requestHandler) =>
         {
            await requestHandler.Invoke();
            httpContexts.Enqueue(httpContext);
         });

         return Create(configureServices, captureHttpContext);
      }

      public static IWebHostBuilder Create(Action<IApplicationBuilder> configureApplication)
      {
         return Create(null, configureApplication);
      }

      public static IWebHostBuilder Create(Action<IServiceCollection> configureServices, Action<IApplicationBuilder> configureApplication)
      {
         Startup app = null;
         var contentRoot = GetContentRoot();

         return new WebHostBuilder()
           .UseContentRoot(contentRoot.FullName)
           .UseEnvironment(EnvironmentName.Development)
           .ConfigureServices(services =>
           {
              hostingEnvironment = GetHostingEnvironment(services);
              app = new Startup(hostingEnvironment);
              ConfigureServices(app, services, configureServices);
           })
           .Configure(builder =>
           {
              ConfigureApplication(app, builder, configureApplication);
           });
      }

      private static DirectoryInfo GetContentRoot()
      {
         const string relativeContentRootPath = @"..\..\..\..\Gtm.WebService.WebApi";
         var contentRoot = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), relativeContentRootPath));

         if (!contentRoot.Exists)
            throw new DirectoryNotFoundException($"Directory '{contentRoot.FullName}' not found.");

         return contentRoot;
      }

      private static void ConfigureServices(Startup app, IServiceCollection services, Action<IServiceCollection> configureServices)
      {
         app.ConfigureServices(services);
         configureServices?.Invoke(services);
      }

      private static IHostingEnvironment GetHostingEnvironment(IServiceCollection services)
      {
         Func<ServiceDescriptor, bool> isHostingEnvironmet = service => service.ImplementationInstance is IHostingEnvironment;
         var hostingEnvironment = (IHostingEnvironment)services.Single(isHostingEnvironmet).ImplementationInstance;
         var assembly = typeof(Startup).GetTypeInfo().Assembly;

         hostingEnvironment.ApplicationName = assembly.GetName().Name;
         return hostingEnvironment;
      }

      private static void ConfigureApplication(Startup app, IApplicationBuilder builder, Action<IApplicationBuilder> configureApplication)
      {
         app.Configure(builder, hostingEnvironment, new LoggerTest());
         configureApplication?.Invoke(builder);
      }

      public class LoggerTest : ILoggerFactory
      {
         public void AddProvider(ILoggerProvider provider)
         {
         }

         public ILogger CreateLogger(string categoryName)
         {
            return null;
         }

         public void Dispose()
         {
         }
      }
   }
}
