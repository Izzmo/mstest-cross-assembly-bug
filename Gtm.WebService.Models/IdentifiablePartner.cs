using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// A model for a partner that has been created within the system.
   /// </summary>
   [DataContract]
   public class IdentifiablePartner : Partner, IIdentifiable, ICloneable<IdentifiablePartner>
   {
      [DataMember(Name = "Identifier")]
      private Guid identifier;

      /// <summary>
      /// Initializes a new instance of the <see cref="IdentifiablePartner"/> class.
      /// </summary>
      /// <param name="identifier">See <see cref="Identifier"/></param>
      /// <param name="name">See <see cref="Partner.Name"/></param>
      /// <param name="authorizationToken">See <see cref="Partner.AuthorizationToken"/></param>
      public IdentifiablePartner(Guid identifier, string name, Guid authorizationToken)
         : base(name, authorizationToken)
      {
         ParameterVerifier.VerifyIsNotEmpty(identifier, nameof(identifier));

         this.identifier = identifier;
      }

      /// <summary>
      /// Gets the identifier of the Partner.
      /// </summary>
      public Guid Identifier => identifier;

      /// <summary>
      /// Clone the <see cref="IdentifiablePartner"/> to a new instance.
      /// </summary>
      public new IdentifiablePartner Clone()
      {
         return new IdentifiablePartner(identifier, Name, AuthorizationToken);
      }
   }
}
