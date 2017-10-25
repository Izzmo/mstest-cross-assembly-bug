using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebApi.TestUtilities.Exceptions
{
   /// <summary>
   /// Exception thrown when an assertion fails on JSON validation.
   /// </summary>
   public class JsonAssertFailedException : AssertFailedException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="JsonAssertFailedException"/> class.
      /// </summary>
      /// <param name="path">The path within the JSON that failed during assertion.</param>
      /// <param name="message">Message for the exception.</param>
      /// <param name="innerException">Optional inner exception.</param>
      public JsonAssertFailedException(string path, string message = null, Exception innerException = null)
         : base((message ?? "The actual value did not match what was expected at the given path.") + $"  Path of mismatch: {path ?? "(unknown)"}", innerException)
      {
         Path = path ?? "(unknown)";
      }

      /// <summary>
      /// Gets the path within the JSON that failed during assertion.
      /// </summary>
      public string Path { get; }
   }
}
