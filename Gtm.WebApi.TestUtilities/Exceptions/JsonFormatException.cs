using System;

namespace Gtm.WebApi.TestUtilities.Exceptions
{
   /// <summary>
   /// An exception to be thrown when JSON did not conform to the expected structure.
   /// </summary>
   public class JsonFormatException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="JsonFormatException"/> class.
      /// </summary>
      public JsonFormatException(string message, string path, Exception innerException = null)
         : base(message + $" Path = {path}", innerException)
      {
         Path = path;
      }

      /// <summary>
      /// Gets the path of the malformed structure.
      /// </summary>
      public string Path { get; }
   }
}
