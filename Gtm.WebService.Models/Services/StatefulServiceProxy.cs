using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Gtm.Common;
using FabricServiceProxy = Microsoft.ServiceFabric.Services.Remoting.Client.ServiceProxy;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// Provides base class for creating service proxies, with convenient methods to handle wrapped exceptions.
   /// </summary>
   /// <typeparam name="TService">Type of <see cref="IService"/> for this proxy.</typeparam>
   public abstract class StatefulServiceProxy<TService> : ServiceProxy<TService>
      where TService : IService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="StatefulServiceProxy{TService}"/> class for the given <paramref name="uri"/>.
      /// </summary>
      /// <param name="uri">The uri for the service.</param>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="uri"/> is <c>null</c>.</exception>
      protected StatefulServiceProxy(Uri uri)
         : base(CreateService(uri))
      {
         ParameterVerifier.VerifyIsNotNull(uri, nameof(uri));
      }

      private static TService CreateService(Uri uri)
      {
         ParameterVerifier.VerifyIsNotNull(uri, nameof(uri));

         return FabricServiceProxy.Create<TService>(uri, new ServicePartitionKey(0));
      }
   }
}
