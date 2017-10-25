using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Fabric.Description;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// The FabricRuntime creates an instance of this class for each service type instance.
   /// </summary>
   internal sealed class WebApi : StatelessService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="WebApi"/> class.
      /// </summary>
      /// <param name="context">The stateless service context.</param>
      public WebApi(StatelessServiceContext context)
          : base(context)
      {
      }

      /// <summary>
      /// Optional override to create listeners (like tcp, http) for this service instance.
      /// </summary>
      /// <returns>The collection of listeners.</returns>
      protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
      {
         var endpoints = Context.CodePackageActivationContext.GetEndpoints()
                        .Where(endpoint => endpoint.Protocol == EndpointProtocol.Http || endpoint.Protocol == EndpointProtocol.Https)
                        .Select(endpoint => endpoint.Name);

         var configPackage = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
         var appInsightsId = configPackage.Settings.Sections["Settings"].Parameters["ApplicationInsightsId"].Value;
         var environment = configPackage.Settings.Sections["Settings"].Parameters["Environment"].Value;

         return endpoints.Select(endpoint => new ServiceInstanceListener(serviceContext =>
            new WebListenerCommunicationListener(serviceContext, endpoint, (url, listener) =>
            {
               ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting WebListener on {url}");

               return new WebHostBuilder().UseWebListener()
                           .ConfigureServices(services => services.AddSingleton(serviceContext))
                           .UseContentRoot(Directory.GetCurrentDirectory())
                           .UseEnvironment(environment)
                           .UseStartup<Startup>()
                           .UseApplicationInsights(appInsightsId)
                           .UseUrls(url)
                           .Build();
            })));
      }
   }
}
