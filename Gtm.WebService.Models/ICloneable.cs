namespace Gtm.WebService.Models
{
   /// <summary>
   /// Specifies the contact for creating a clonable type.
   /// </summary>
   /// <typeparam name="T">The type to be able to clone.</typeparam>
   public interface ICloneable<out T>
   {
      /// <summary>
      /// Clones a type.
      /// </summary>
      /// <returns>A cloned new instance of the type.</returns>
      T Clone();
   }
}
