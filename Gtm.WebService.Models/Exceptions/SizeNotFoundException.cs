using System;
using System.Runtime.Serialization;

namespace Gtm.WebService.Models.Exceptions
{
   /// <summary>
   /// Exception for When a Size Is Not Found Upon Lookup.
   /// </summary>
   [Serializable]
   public class SizeNotFoundException : ArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="SizeNotFoundException"/> class.
      /// </summary>
      public SizeNotFoundException()
         : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SizeNotFoundException"/> class.
      /// </summary>
      public SizeNotFoundException(Guid designIdentifier, string size)
         : base($"The size {size} was not found for design {designIdentifier.ToString("N")}.")
      {
         DesignIdentifier = designIdentifier;
         Size = size;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SizeNotFoundException"/> class.
      /// </summary>
      protected SizeNotFoundException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Gets the identifier of the design that was fetched.
      /// </summary>
      public Guid DesignIdentifier { get; }

      /// <summary>
      /// Gets the size that was not found.
      /// </summary>
      public string Size { get; }
   }
}
