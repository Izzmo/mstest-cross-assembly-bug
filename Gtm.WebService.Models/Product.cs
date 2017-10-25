using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// A Product
   /// </summary>
   [DataContract]
   public class Product : ICloneable<Product>
   {
      [DataMember(Name = "Color")]
      private string color;

      [DataMember(Name = "Name")]
      private string name;

      [DataMember(Name = "Sizes")]
      private IDictionary<string, Size> sizes;

      [DataMember(Name = "Style")]
      private string style;

      /// <summary>
      /// Initializes a new instance of the <see cref="Product"/> class.
      /// </summary>
      /// <param name="name">See <see cref="Name"/></param>
      /// <param name="style">See <see cref="Style"/></param>
      /// <param name="color">See <see cref="Color"/></param>
      /// <param name="sizes">See <see cref="Sizes"/></param>
      /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="sizes"/> has an empty count.</exception>
      /// <exception cref="ArgumentNullException">Thrown if any parameter is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">Thrown if any parameters are invalid.</exception>
      public Product(string name, string style, string color, IDictionary<string, Size> sizes)
      {
         ParameterVerifier.VerifyIsNotNullOrEmpty(name, nameof(name));
         ParameterVerifier.VerifyIsNotNullOrEmpty(color, nameof(color));
         ParameterVerifier.VerifyIsNotNull(sizes, nameof(sizes));
         ParameterVerifier.VerifyIsAtLeast(sizes.Count, 1, nameof(sizes));
         ParameterVerifier.VerifyIsNotNullOrEmpty(style, nameof(style));

         this.name = name;
         this.color = color;
         this.sizes = sizes;
         this.style = style;
      }

      /// <summary>
      /// Gets the name of the product.
      /// </summary>
      public string Name => name;

      /// <summary>
      /// Gets the color name of the product.
      /// </summary>
      public string Color => color;

      /// <summary>
      /// Gets the list of sizes for the design.
      /// </summary>
      public IDictionary<string, Size> Sizes => sizes;

      /// <summary>
      /// Gets the style code of the product.
      /// </summary>
      public string Style => style;

      /// <summary>
      /// Clone the <see cref="Product"/> to a new instance.
      /// </summary>
      /// <returns></returns>
      public Product Clone()
      {
         return new Product(name, style, color, new Dictionary<string, Size>(sizes));
      }

      /// <summary>
      /// Determines if the product is equal to another.
      /// </summary>
      /// <param name="other">The other product to compare.</param>
      /// <returns><c>true</c> if equal, <c>false</c> otherwise.</returns>
      public bool Equals(Product other)
      {
         foreach (var size in sizes)
         {
            if (!other.sizes.ContainsKey(size.Key) || other.sizes[size.Key].Price != size.Value.Price)
               return false;
         }

         return name == other.Name
            && color == other.Color
            && style == other.Style;
      }

      /// <summary>
      /// Generates a hash code based on the <see cref="Name"/>, <see cref="Color"/>, and <see cref="Style"/> of the product.
      /// </summary>
      /// <returns>Integer hash code</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hash = 13;

            hash = (hash * 7) + name.GetHashCode();
            hash = (hash * 7) + color.GetHashCode();
            hash = (hash * 7) + style.GetHashCode();

            return hash;
         }
      }
   }
}
