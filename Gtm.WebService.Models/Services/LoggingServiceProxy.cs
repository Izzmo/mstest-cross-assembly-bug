using System;
using System.Threading.Tasks;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// Proxy class for <see cref="ILoggingService"/>
   /// </summary>
   public class LoggingServiceProxy : StatefulServiceProxy<ILoggingService>, ILoggingService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="LoggingServiceProxy"/> class.
      /// </summary>
      public LoggingServiceProxy()
         : base(new Uri("fabric:/Gtm.WebService/LoggingService"))
      {
      }

      /// <summary>
      /// <see cref="ILoggingService.LogError(string, string, string)"/>
      /// </summary>
      public Task LogError(string service, string eventName, string serializedJsonException)
      {
         return InvokeService(() => Service.LogError(service, eventName, serializedJsonException));
      }
   }
}
