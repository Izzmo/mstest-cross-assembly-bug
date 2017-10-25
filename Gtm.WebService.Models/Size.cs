using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// Data structure that represents a size.
   /// </summary>
   [DataContract]
   public class Size : ICloneable<Size>
   {
      [DataMember(Name = "Name")]
      private string name;

      [DataMember(Name = "Price")]
      private decimal price;

      /// <summary>
      /// Initializes a new instance of the <see cref="Size"/> class.
      /// </summary>
      /// <param name="name">The name of the size, such as "M." It should not be a phonetic name, such as Medium. <see cref="Name"/>.</param>
      /// <param name="price">The price of the size.</param>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is empty, or if <paramref name="price"/> is less than or equal to 0.</exception>
      public Size(string name, decimal price)
      {
         ParameterVerifier.VerifyIsNotNullOrEmpty(name, nameof(name));
         ParameterVerifier.VerifyIsAtLeast(price, 0.01m, nameof(price));

         this.name = name;
         this.price = price;
      }

      /// <summary>
      /// Gets the name of the size.
      /// </summary>
      /// <example>M</example>
      /// <remarks>Should not be the phonetic name of the size.</remarks>
      public string Name => name;

      /// <summary>
      /// Gets the price of the size.
      /// </summary>
      public decimal Price => price;

      /// <summary>
      /// Clone the <see cref="Size"/> to a new instance.
      /// </summary>
      public Size Clone()
      {
         return new Size(name, price);
      }
   }
}
