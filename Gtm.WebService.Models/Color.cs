using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// A color.
   /// </summary>
   [DataContract]
   public class Color : ICloneable<Color>
   {
      [DataMember(Name = "HexCode")]
      private readonly string hexCode;

      [DataMember(Name = "Name")]
      private readonly string name;

      /// <summary>
      /// Initializes a new instance of the <see cref="Color"/> class.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="hexCode"/> is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> or <paramref name="hexCode"/> is empty.</exception>
      public Color(string name, string hexCode)
      {
         ParameterVerifier.VerifyIsNotNullOrEmpty(name, nameof(name));
         ParameterVerifier.VerifyIsNotNullOrEmpty(hexCode, nameof(hexCode));

         this.name = name;
         this.hexCode = hexCode;
      }

      /// <summary>
      /// Gets the hex code of the color.
      /// </summary>
      public string HexCode => hexCode;

      /// <summary>
      /// Gets the name of the color.
      /// </summary>
      public string Name => name;

      /// <summary>
      /// Clone the <see cref="Color"/> to a new instance.
      /// </summary>
      public Color Clone()
      {
         return new Color(name, hexCode);
      }
   }
}
