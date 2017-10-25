using System;
using System.Runtime.Serialization;
using Gtm.Common;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// Image URLs for a product.
   /// </summary>
   [DataContract]
   public class ImageUrls : ICloneable<ImageUrls>
   {
      [DataMember(Name = "Back")]
      private Uri back;

      [DataMember(Name = "Front")]
      private Uri front;

      [DataMember(Name = "Left")]
      private Uri left;

      [DataMember(Name = "Right")]
      private Uri right;

      /// <summary>
      /// Initializes a new instance of the <see cref="ImageUrls"/> class.
      /// </summary>
      /// <param name="back">See <see cref="Back"/></param>
      /// <param name="front">See <see cref="Front"/></param>
      /// <param name="left">See <see cref="Left"/></param>
      /// <param name="right">See <see cref="Right"/></param>
      /// <exception cref="ArgumentNullException">Thrown if any passed in parameter is <c>null</c>.</exception>
      public ImageUrls(Uri back, Uri front, Uri left, Uri right)
      {
         ParameterVerifier.VerifyIsNotNull(back, nameof(back));
         ParameterVerifier.VerifyIsNotNull(front, nameof(front));
         ParameterVerifier.VerifyIsNotNull(left, nameof(left));
         ParameterVerifier.VerifyIsNotNull(right, nameof(right));

         this.back = back;
         this.front = front;
         this.left = left;
         this.right = right;
      }

      /// <summary>
      /// Gets the back image.
      /// </summary>
      public Uri Back => back;

      /// <summary>
      /// Gets the front image.
      /// </summary>
      public Uri Front => front;

      /// <summary>
      /// Gets the left image.
      /// </summary>
      public Uri Left => left;

      /// <summary>
      /// Gets the right image.
      /// </summary>
      public Uri Right => right;

      /// <summary>
      /// Clone the <see cref="ImageUrls"/> to a new instance.
      /// </summary>
      public ImageUrls Clone()
      {
         return new ImageUrls(back, front, left, right);
      }
   }
}
