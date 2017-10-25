using System;
using System.Collections.Generic;
using System.Linq;
using Gtm.WebApi.TestUtilities.Exceptions;

namespace Gtm.WebApi.TestUtilities
{
   /// <summary>
   /// Provides convenient extension method to assert that a JSON string contains
   /// what was expected.
   /// </summary>
   public static class AssertJson
   {
      /// <summary>
      ///   Asserts that the string in the form of JSON contains the structure and values
      ///   of what is expected, giving option for external asserters.
      /// </summary>
      /// <param name="expected">An object of what is expected.</param>
      /// <param name="actualJson">JSON of what needs verified.</param>
      /// <param name="valueAsserters">
      ///   Delegates accepting the actual value encountered and returns whether assertion should succeed.
      /// </param>
      /// <exception cref="JsonAssertFailedException">
      ///   Thrown if <paramref name="actualJson"/> does not match <paramref name="expected"/>.
      /// </exception>
      /// <remarks>
      ///   The <paramref name="expected"/> parameter is of type <see cref="object"/> so
      ///   that it safely accepts anonymous types.  This allows for clean test code.
      /// </remarks>
      /// <example>
      ///   Below is an example of how the optional external asserters can be used.
      ///   <code>
      ///     // The below string a would usually be a string from an WebResponse.
      ///     var a = @"{""CustomerId"":123,""Name"":""Non-empty string""}";
      ///
      ///     var e = new
      ///     {
      ///        CustomerId = "{0}",
      ///        FirstName = "{1}"
      ///     };
      ///
      ///     a.Matches(e, a => (int)a > 0, a => !string.IsNullOrEmpty((string)a));
      ///   </code>
      /// </example>
      public static void Matches(object expected, string actualJson, params Func<object, bool>[] valueAsserters)
      {
         var asserter = new JsonAsserter(valueAsserters);

         asserter.AssertMatchesExpected(new JsonValue(actualJson), new JsonValue(expected));
      }

      /// <summary>
      ///   Asserts that the <see cref="JsonValue"/> contains the structure and values
      ///   of what is expected, giving option for external asserters.
      /// </summary>
      /// <param name="expected">An object of what is expected.</param>
      /// <param name="actualJson">JSON of what needs verified.</param>
      /// <param name="valueAsserters">
      ///   Delegates accepting the actual value encountered and returns whether assertion should succeed.
      /// </param>
      /// <exception cref="JsonAssertFailedException">
      ///   Thrown if <paramref name="actualJson"/> does not match <paramref name="expected"/>.
      /// </exception>
      /// <remarks>
      ///   The <paramref name="expected"/> parameter is of type <see cref="object"/> so
      ///   that it safely accepts anonymous types.  This allows for clean test code.
      /// </remarks>
      /// <example>
      ///   Below is an example of how the optional external asserters can be used.
      ///   <code>
      ///     // The below string a would usually be a string from an WebResponse.
      ///     var a = new JsonValue(@"{""CustomerId"":123,""Name"":""Non-empty string""}");
      ///
      ///     var e = new
      ///     {
      ///        CustomerId = "{0}",
      ///        FirstName = "{1}"
      ///     };
      ///
      ///     a.Matches(e, a => (int)a > 0, a => !string.IsNullOrEmpty((string)a));
      ///   </code>
      /// </example>
      public static void Matches(object expected, JsonValue actualJson, params Func<object, bool>[] valueAsserters)
      {
         var asserter = new JsonAsserter(valueAsserters);

         asserter.AssertMatchesExpected(actualJson, new JsonValue(expected));
      }

      /// <summary>
      ///   Asserts that the <see cref="IEnumerable{T}"/> of <see cref="JsonValue"/>
      ///   contains the structure and values of what is expected, giving option for external asserters.
      /// </summary>
      /// <param name="expected">An object of what is expected.</param>
      /// <param name="actualJsonList">JSON of what needs verified.</param>
      /// <param name="valueAsserters">
      ///   Delegates accepting the actual value encountered and returns whether assertion should succeed.
      /// </param>
      /// <exception cref="JsonAssertFailedException">
      ///   Thrown if <paramref name="actualJsonList"/> does not match <paramref name="expected"/>.
      /// </exception>
      /// <remarks>
      ///   The <paramref name="expected"/> parameter is of type <see cref="object"/> so
      ///   that it safely accepts anonymous types.  This allows for clean test code.
      /// </remarks>
      /// <seealso cref="Matches(object, JsonValue, Func{object, bool}[])"/>
      public static void Matches(object[] expected, IEnumerable<JsonValue> actualJsonList, params Func<object, bool>[] valueAsserters)
      {
         var asserter = new JsonAsserter(valueAsserters);

         asserter.AssertMatchesExpected(actualJsonList, expected.Select(o => new JsonValue(o)));
      }
   }
}
