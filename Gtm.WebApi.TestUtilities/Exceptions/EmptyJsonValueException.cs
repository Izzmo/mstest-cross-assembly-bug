namespace Gtm.WebApi.TestUtilities.Exceptions
{
   /// <summary>
   /// Thrown when a value was missing at a specific location while parsing the JSON.
   /// </summary>
   public class EmptyJsonValueException : JsonFormatException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="EmptyJsonValueException"/> class.
      /// </summary>
      /// <param name="path">Path to the location of the empty value.</param>
      public EmptyJsonValueException(string path)
         : base("A value was requested at the given path but the actual value is empty.", path)
      {
      }
   }
}
