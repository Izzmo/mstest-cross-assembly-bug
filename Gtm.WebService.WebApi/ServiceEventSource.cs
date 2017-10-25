using System;
using System.Diagnostics.Tracing;
using System.Fabric;
using System.Threading.Tasks;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// Service event source
   /// </summary>
   [EventSource(Name = "Gtm.WebService-Web")]
   internal sealed class ServiceEventSource : EventSource
   {
      /// <summary>
      /// Current event source
      /// </summary>
      public static readonly ServiceEventSource Current = new ServiceEventSource();

      private const int MessageEventId = 1;
      private const int ServiceMessageEventId = 2;
      private const int ServiceTypeRegisteredEventId = 3;
      private const int ServiceHostInitializationFailedEventId = 4;
      private const int ServiceRequestStartEventId = 5;
      private const int ServiceRequestStopEventId = 6;

      static ServiceEventSource()
      {
         Task.Run(() => { });
      }

      private ServiceEventSource()
         : base()
      {
      }

      #region Events

      /// <summary>
      /// Service message
      /// </summary>
      [NonEvent]
      public void Message(string message, params object[] args)
      {
         if (IsEnabled())
         {
            string finalMessage = string.Format(message, args);
            Message(finalMessage);
         }
      }

      /// <summary>
      /// Service message
      /// </summary>
      [Event(MessageEventId, Level = EventLevel.Informational, Message = "{0}")]
      public void Message(string message)
      {
         if (IsEnabled())
         {
            WriteEvent(MessageEventId, message);
         }
      }

      /// <summary>
      /// Service message
      /// </summary>
      [NonEvent]
      public void ServiceMessage(ServiceContext serviceContext, string message, params object[] args)
      {
         if (IsEnabled())
         {
            string finalMessage = string.Format(message, args);
            ServiceMessage(
                serviceContext.ServiceName.ToString(),
                serviceContext.ServiceTypeName,
                GetReplicaOrInstanceId(serviceContext),
                serviceContext.PartitionId,
                serviceContext.CodePackageActivationContext.ApplicationName,
                serviceContext.CodePackageActivationContext.ApplicationTypeName,
                serviceContext.NodeContext.NodeName,
                finalMessage);
         }
      }

      /// <summary>
      /// Service Initialization Registered Event Callback
      /// </summary>
      [Event(ServiceTypeRegisteredEventId, Level = EventLevel.Informational, Message = "Service host process {0} registered service type {1}", Keywords = Keywords.ServiceInitialization)]
      public void ServiceTypeRegistered(int hostProcessId, string serviceType)
      {
         WriteEvent(ServiceTypeRegisteredEventId, hostProcessId, serviceType);
      }

      /// <summary>
      /// Service Initialization Failure Event Callback
      /// </summary>
      [Event(ServiceHostInitializationFailedEventId, Level = EventLevel.Error, Message = "Service host initialization failed", Keywords = Keywords.ServiceInitialization)]
      public void ServiceHostInitializationFailed(string exception)
      {
         WriteEvent(ServiceHostInitializationFailedEventId, exception);
      }

      /// <summary>
      /// Called at start of service request
      /// </summary>
      [Event(ServiceRequestStartEventId, Level = EventLevel.Informational, Message = "Service request '{0}' started", Keywords = Keywords.Requests)]
      public void ServiceRequestStart(string requestTypeName)
      {
         WriteEvent(ServiceRequestStartEventId, requestTypeName);
      }

      /// <summary>
      /// Called at end of service request
      /// </summary>
      [Event(ServiceRequestStopEventId, Level = EventLevel.Informational, Message = "Service request '{0}' finished", Keywords = Keywords.Requests)]
      public void ServiceRequestStop(string requestTypeName, string exception = "")
      {
         WriteEvent(ServiceRequestStopEventId, requestTypeName, exception);
      }

      private static long GetReplicaOrInstanceId(ServiceContext context)
      {
         StatelessServiceContext stateless = context as StatelessServiceContext;
         if (stateless != null)
         {
            return stateless.InstanceId;
         }

         StatefulServiceContext stateful = context as StatefulServiceContext;
         if (stateful != null)
         {
            return stateful.ReplicaId;
         }

         throw new NotSupportedException("Context type not supported.");
      }
#if UNSAFE
        private int SizeInBytes(string s)
        {
            if (s == null)
            {
                return 0;
            }
            else
            {
                return (s.Length + 1) * sizeof(char);
            }
        }
#endif

      [Event(ServiceMessageEventId, Level = EventLevel.Informational, Message = "{7}")]
      private
#if UNSAFE
        unsafe
#endif
        void ServiceMessage(
          string serviceName,
          string serviceTypeName,
          long replicaOrInstanceId,
          Guid partitionId,
          string applicationName,
          string applicationTypeName,
          string nodeName,
          string message)
      {
#if !UNSAFE
         WriteEvent(ServiceMessageEventId, serviceName, serviceTypeName, replicaOrInstanceId, partitionId, applicationName, applicationTypeName, nodeName, message);
#else
            const int numArgs = 8;
            fixed (char* pServiceName = serviceName, pServiceTypeName = serviceTypeName, pApplicationName = applicationName, pApplicationTypeName = applicationTypeName, pNodeName = nodeName, pMessage = message)
            {
                EventData* eventData = stackalloc EventData[numArgs];
                eventData[0] = new EventData { DataPointer = (IntPtr) pServiceName, Size = SizeInBytes(serviceName) };
                eventData[1] = new EventData { DataPointer = (IntPtr) pServiceTypeName, Size = SizeInBytes(serviceTypeName) };
                eventData[2] = new EventData { DataPointer = (IntPtr) (&replicaOrInstanceId), Size = sizeof(long) };
                eventData[3] = new EventData { DataPointer = (IntPtr) (&partitionId), Size = sizeof(Guid) };
                eventData[4] = new EventData { DataPointer = (IntPtr) pApplicationName, Size = SizeInBytes(applicationName) };
                eventData[5] = new EventData { DataPointer = (IntPtr) pApplicationTypeName, Size = SizeInBytes(applicationTypeName) };
                eventData[6] = new EventData { DataPointer = (IntPtr) pNodeName, Size = SizeInBytes(nodeName) };
                eventData[7] = new EventData { DataPointer = (IntPtr) pMessage, Size = SizeInBytes(message) };

                WriteEventCore(ServiceMessageEventId, numArgs, eventData);
            }
#endif
      }
      #endregion

      #region Keywords

      /// <summary>
      /// Event keywords can be used to categorize events.
      /// </summary>
      /// <remarks>
      /// Each keyword is a bit flag. A single event can be associated with multiple keywords (via EventAttribute.Keywords property).
      /// Keywords must be defined as a public class named 'Keywords' inside EventSource that uses them.
      /// </remarks>
      public static class Keywords
      {
         /// <summary>
         /// Requests location
         /// </summary>
         public const EventKeywords Requests = (EventKeywords)0x1L;

         /// <summary>
         /// Service Initialization location
         /// </summary>
         public const EventKeywords ServiceInitialization = (EventKeywords)0x2L;
      }
      #endregion
   }
}
