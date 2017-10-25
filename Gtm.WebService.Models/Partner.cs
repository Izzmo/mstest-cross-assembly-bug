using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// A model for a Partner.
   /// </summary>
   [DataContract]
   public class Partner : ICloneable<Partner>
   {
      [DataMember(Name = "AuthorizationToken")]
      private Guid authorizationToken;

      [DataMember(Name = "Name")]
      private string name;

      /// <summary>
      /// Initializes a new instance of the <see cref="Partner"/> class.
      /// </summary>
      /// <param name="name">See <see cref="Name"/></param>
      /// <param name="authorizationToken">See <see cref="AuthorizationToken"/></param>
      public Partner(string name, Guid authorizationToken)
      {
         ParameterVerifier.VerifyIsNotNullOrEmpty(name, nameof(name));
         ParameterVerifier.VerifyIsNotEmpty(authorizationToken, nameof(authorizationToken));

         this.name = name;
         this.authorizationToken = authorizationToken;
      }

      /// <summary>
      /// Gets the authorization token (guid) of the Partner.
      /// </summary>
      public Guid AuthorizationToken => authorizationToken;

      /// <summary>
      /// Gets the name of the Partner.
      /// </summary>
      public string Name => name;

      /// <summary>
      /// Clone the <see cref="Partner"/> to a new instance.
      /// </summary>
      public Partner Clone()
      {
         return new Partner(name, authorizationToken);
      }
   }
}
