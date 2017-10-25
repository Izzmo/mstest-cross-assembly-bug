using System;
using System.Runtime.Serialization;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.Models.Exceptions
{
   /// <summary>
   /// An exception to be thrown by the <see cref="IPaymentService"/>.
   /// </summary>
   [Serializable]
   public class UnauthorizedPaymentException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="UnauthorizedPaymentException"/> class.
      /// </summary>
      public UnauthorizedPaymentException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="UnauthorizedPaymentException"/> class.
      /// </summary>
      /// <param name="info">The serialization information</param>
      /// <param name="context">The context of the serializer</param>
      public UnauthorizedPaymentException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
