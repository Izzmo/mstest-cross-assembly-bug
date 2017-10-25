using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// The main entry point for the WebApi.
   /// </summary>
   internal static class Program
   {
      /// <summary>
      /// This is the entry point of the service host process.
      /// </summary>
      private static void Main()
      {
         try
         {
            ServiceRuntime.RegisterServiceAsync(
               "WebType",
                context => new WebApi(context)).GetAwaiter().GetResult();

            ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebApi).Name);

            Thread.Sleep(Timeout.Infinite);
         }
         catch (Exception e)
         {
            ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            throw;
         }
      }
   }
}
