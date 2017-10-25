using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// Data structure representing a market.
   /// </summary>
   [DataContract]
   public class Market : ICloneable<Market>
   {
      [DataMember(Name = "MarketCode")]
      private string marketCode;

      [DataMember(Name = "Name")]
      private string name;

      [DataMember(Name = "ShortName")]
      private string shortName;

      /// <summary>
      /// Initializes a new instance of the <see cref="Market"/> class.
      /// </summary>
      /// <param name="marketCode"><see cref="MarketCode"/></param>
      /// <param name="name"><see cref="Name"/></param>
      /// <param name="shortName">See <see cref="ShortName"/></param>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="marketCode"/>, <paramref name="name"/>, or <paramref name="shortName"/> is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">Thrown if <paramref name="marketCode"/> is invalid, or if <paramref name="name"/> or <paramref name="shortName"/> is passed in empty.</exception>
      public Market(string marketCode, string name, string shortName)
      {
         ParameterVerifier.VerifyMatchesPattern(marketCode, @"^[a-z]{3}$", nameof(marketCode));
         ParameterVerifier.VerifyIsNotNullOrEmpty(name, nameof(name));
         ParameterVerifier.VerifyIsNotNullOrEmpty(shortName, nameof(shortName));

         this.marketCode = marketCode.ToUpper();
         this.name = name;
         this.shortName = shortName;
      }

      /// <summary>
      /// Gets the code of the market.
      /// </summary>
      public string MarketCode => marketCode;

      /// <summary>
      /// Gets the name of the market.
      /// </summary>
      public string Name => name;

      /// <summary>
      /// Gets the short name of the market.
      /// </summary>
      public string ShortName => shortName;

      /// <summary>
      /// Clone the <see cref="Market"/> to a new instance.
      /// </summary>
      public Market Clone()
      {
         return new Market(marketCode, name, shortName);
      }
   }
}
