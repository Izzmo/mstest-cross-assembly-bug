using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Gtm.Common;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// Provides base class for creating service proxies, with convenient methods to handle wrapped exceptions.
   /// </summary>
   /// <typeparam name="TService">Type of <see cref="IService"/> for this proxy.</typeparam>
   public abstract class ServiceProxy<TService>
      where TService : IService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ServiceProxy{TService}"/> class for the given <paramref name="service"/>.
      /// </summary>
      /// <param name="service">The service.</param>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="service"/> is <c>null</c>.</exception>
      protected ServiceProxy(TService service)
      {
         ParameterVerifier.VerifyIsNotNull(service, nameof(service));

         Service = service;
      }

      /// <summary>
      /// Gets the service provided.
      /// </summary>
      protected TService Service { get; private set; }

      /// <summary>
      /// Invokes a service function with built-in exception handling, unwrapping
      /// an inner exception from a possible <see cref="AggregateException"/>.
      /// </summary>
      /// <typeparam name="TReturn">The return type of the service function.</typeparam>
      /// <param name="serviceFunction">A function to invoke.</param>
      /// <returns>
      /// Returns the result of successful invocation of the given <paramref name="serviceFunction"/>.
      /// </returns>
      protected async Task<TReturn> InvokeService<TReturn>(Func<Task<TReturn>> serviceFunction)
      {
         try
         {
            return await serviceFunction();
         }
         catch (AggregateException ex) when (ex.InnerException != null)
         {
            throw ex.InnerException;
         }
      }

      /// <summary>
      /// Invokes a service method with built-in exception handling, unwrapping
      /// an inner exception from a possible <see cref="AggregateException"/>.
      /// </summary>
      /// <param name="serviceFunction">A method to invoke.</param>
      protected async Task InvokeService(Func<Task> serviceFunction)
      {
         try
         {
            await serviceFunction();
         }
         catch (AggregateException ex) when (ex.InnerException != null)
         {
            throw ex.InnerException;
         }
      }
   }
}
