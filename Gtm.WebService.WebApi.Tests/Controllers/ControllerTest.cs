using Microsoft.Extensions.DependencyInjection;
using Gtm.WebService.Models.Services;
using Gtm.WebService.Models.Tests.Services.PartnerService;

namespace Gtm.WebService.WebApi.Tests.Controllers
{
   /// <summary>
   /// A base class for MVC Controller Unit Tests.
   /// This allows for easy implementation of dependency-injection of services.
   /// </summary>
   /// <typeparam name="TService">The interface of the service to be injected</typeparam>
   /// <typeparam name="TImplementation">The implementation of the <typeparamref name="TService"/></typeparam>
   public class ControllerTest<TService, TImplementation> : WebApiTest<TService, TImplementation>
      where TService : class
      where TImplementation : class, TService
   {
      /// <summary>
      /// Add service dependencies to the controller test.
      /// </summary>
      /// <remarks>The <see cref="FakePartnerService"/> is included by default for authorization.</remarks>
      /// <param name="services">The collection of service dependencies.</param>
      protected override void AddServices(IServiceCollection services)
      {
         services.AddTransient<IPartnerService, FakePartnerService>();
         services.AddSingleton<TService, TImplementation>();
      }
   }

   /// <summary>
   /// A base class for MVC Controller Unit Tests.
   /// </summary>
   public class ControllerTest : WebApiTest
   {
      /// <summary>
      /// Add service dependencies to the controller test.
      /// </summary>
      /// <remarks>The <see cref="FakePartnerService"/> is included by default for authorization.</remarks>
      /// <param name="services">The collection of service dependencies.</param>
      protected override void AddServices(IServiceCollection services)
      {
         services.AddTransient<IPartnerService, FakePartnerService>();
      }
   }
}
