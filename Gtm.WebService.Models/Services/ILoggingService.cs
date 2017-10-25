using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// The interface for the LoggingService.
   /// </summary>
   public interface ILoggingService : IService
   {
      /// <summary>
      /// Log an error.
      /// </summary>
      /// <param name="service">The name of the service the error originated from.</param>
      /// <param name="eventName">The name of the event the error originated from.</param>
      /// <param name="serializedJsonException">The exception that occurred, serialized in JSON.</param>
      Task LogError(string service, string eventName, string serializedJsonException);
   }
}
