using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// Data structure that represents a design.
   /// </summary>
   [DataContract]
   public class Design : IIdentifiable, ICreatable, ICloneable<Design>
   {
      [DataMember(Name = "Created")]
      private VersionInfo created;

      [DataMember(Name = "Identifier")]
      private Guid identifier;

      [DataMember(Name = "ImageUrls")]
      private ImageUrls imageUrls;

      [DataMember(Name = "IsEmbellished")]
      private bool isEmbellished;

      [DataMember(Name = "Product")]
      private Product product;

      /// <summary>
      /// Initializes a new instance of the <see cref="Design"/> class.
      /// </summary>
      /// <param name="identifier">See <see cref="Identifier"/></param>
      /// <param name="product">See <see cref="Product"/></param>
      /// <param name="imageUrls">See <see cref="ImageUrls"/></param>
      /// <param name="createdInfo">See <see cref="Created"/></param>
      /// <param name="isEmbellished">See <see cref="IsEmbellished"/></param>
      /// <exception cref="ArgumentNullException">
      /// Thrown if any parameters are <c>null</c>:
      ///   <list type="bullet">
      ///      <item><paramref name="product"/></item>
      ///      <item><paramref name="imageUrls"/></item>
      ///      <item><paramref name="createdInfo"/></item>
      ///   </list>
      /// </exception>
      /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> is empty.</exception>
      public Design(Guid identifier, Product product, ImageUrls imageUrls, VersionInfo createdInfo, bool isEmbellished = false)
      {
         ParameterVerifier.VerifyIsNotEmpty(identifier, nameof(identifier));
         ParameterVerifier.VerifyIsNotNull(product, nameof(product));
         ParameterVerifier.VerifyIsNotNull(imageUrls, nameof(imageUrls));
         ParameterVerifier.VerifyIsNotNull(createdInfo, nameof(createdInfo));

         this.identifier = identifier;
         this.product = product;
         this.imageUrls = imageUrls;
         created = createdInfo;
         this.isEmbellished = isEmbellished;
      }

      /// <summary>
      /// Gets information about when the design was created.
      /// </summary>
      public VersionInfo Created => created;

      /// <summary>
      /// Gets the unique identifier of the design.
      /// </summary>
      public Guid Identifier => identifier;

      /// <summary>
      /// Gets the list of images for the design.
      /// </summary>
      public ImageUrls ImageUrls => imageUrls;

      /// <summary>
      /// Gets a value indicating whether or not the design is embellished.
      /// </summary>
      public bool IsEmbellished => isEmbellished;

      /// <summary>
      /// Gets the product of the design.
      /// </summary>
      public Product Product => product;

      /// <summary>
      /// Clone the <see cref="Design"/> to a new instance.
      /// </summary>
      public Design Clone()
      {
         return new Design(identifier, product.Clone(), ImageUrls.Clone(), new VersionInfo(created.By, created.On), isEmbellished);
      }
   }
}
