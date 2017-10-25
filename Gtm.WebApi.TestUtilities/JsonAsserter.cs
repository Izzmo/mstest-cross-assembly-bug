using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Gtm.Common;
using Gtm.WebApi.TestUtilities.Exceptions;

namespace Gtm.WebApi.TestUtilities
{
   /// <summary>
   ///   Object to help assert that a string of expected JSON value(s) matches that
   ///   of what is expected.
   /// </summary>
   public class JsonAsserter
   {
      private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
      private readonly Func<object, bool>[] valueAsserters;

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonAsserter"/> class.
      /// </summary>
      /// <param name="valueAsserters">External asserters that can be used for asserting values match what was expected.</param>
      /// <example>
      ///   Below is an example of how the optional external asserters can be used.
      ///   <code>
      ///     // The below string a would usually be a string from an WebResponse.
      ///     var a = @"{""CustomerId"":123,""Name"":""Non-empty string"",""ExistingValue"":true}";
      ///
      ///     var e = new
      ///     {
      ///        CustomerId = "{0}",
      ///        FirstName = "{1}",
      ///        ExistingValue = "{}" // Empty indicates no special asserter, just assert property exists.
      ///     };
      ///
      ///     var asserter = new JsonAsserter(a => (int)a > 0, a => !string.IsNullOrEmpty((string)a));
      ///     asserter.AssertMatchesExpected(a, e);
      ///   </code>
      /// </example>
      public JsonAsserter(Func<object, bool>[] valueAsserters = null)
      {
         this.valueAsserters = valueAsserters ?? new Func<object, bool>[0];
      }

      /// <summary>
      ///   Asserts that the <paramref name="expected"/> object matches the JSON object in <paramref name="actual"/>.
      /// </summary>
      /// <param name="actual">The actual JSON to match.</param>
      /// <param name="expected">The expected object.</param>
      /// <exception cref="ArgumentNullException">
      ///   Thrown if <paramref name="actual"/> or <paramref name="expected"/> are <c>null</c>.
      /// </exception>
      /// <exception cref="JsonAssertFailedException">
      ///   Thrown if <paramref name="actual"/> does not match <paramref name="expected"/>.
      /// </exception>
      /// <example>
      ///   The below example shows how this method could be used in a test class, utilizing anonymous types for C# readability.
      ///   <code>
      ///     // The below string a would usually be a string from an WebResponse.
      ///     var a = @"{""CustomerId"":123,""FirstName"":""John"",""LastName"":""Doe"",""PhoneNumbers"":[{""Type"":""Work"",""Value"":""1234567890""}]}";
      ///
      ///     var e = new
      ///     {
      ///        CustomerId = 123,
      ///        FirstName = "John",
      ///        LastName = "Doe",
      ///        PhoneNumbers = new[]
      ///        {
      ///           new
      ///           {
      ///              Type = "Work",
      ///              Value = "1234567890"
      ///           }
      ///        }
      ///     };
      ///
      ///     var asserter = new JsonAsserter();
      ///     asserter.AssertMatchesExpected(new JsonValue(a), new JsonValue(e));
      ///   </code>
      /// </example>
      public void AssertMatchesExpected(JsonValue actual, JsonValue expected)
      {
         ParameterVerifier.VerifyIsNotNull(actual, nameof(actual));
         ParameterVerifier.VerifyIsNotNull(expected, nameof(expected));

         AssertActualMatchesExpected(actual.Value, expected.Value, expected.Path);
      }

      /// <summary>
      ///   Asserts that the <paramref name="expected"/> objects matches the JSON values in <paramref name="actual"/>.
      /// </summary>
      /// <param name="actual">The actual JSON objects to match.</param>
      /// <param name="expected">The expected objects.</param>
      /// <exception cref="ArgumentNullException">
      ///   Thrown if <paramref name="actual"/> or <paramref name="expected"/> are <c>null</c>.
      /// </exception>
      /// <exception cref="JsonAssertFailedException">
      ///   Thrown if <paramref name="actual"/> does not match <paramref name="expected"/>.
      /// </exception>
      /// <seealso cref="AssertMatchesExpected(JsonValue, JsonValue)"/>
      public void AssertMatchesExpected(IEnumerable<JsonValue> actual, IEnumerable<JsonValue> expected)
      {
         ParameterVerifier.VerifyIsNotNull(actual, nameof(actual));
         ParameterVerifier.VerifyIsNotNull(expected, nameof(expected));

         using (var e = expected.GetEnumerator())
         {
            var i = 0;

            foreach (var a in actual)
            {
               if (!e.MoveNext())
                  throw new JsonAssertFailedException(JsonValue.RootPath, "The actual result had more entries than what was expected.");

               AssertActualMatchesExpected(a.Value, e.Current?.Value, $"{e.Current?.Path}[{i++}]");
            }

            if (e.MoveNext())
               throw new JsonAssertFailedException(JsonValue.RootPath, "The actual result had fewer entries than what was expected.");
         }
      }

      private static void AssertObjectsAreEqual(object expected, object actual, string path)
      {
         if (actual != null && actual.GetType() != expected.GetType() && actual is IConvertible)
            actual = Convert.ChangeType(actual, expected.GetType());

         AssertIsTrue(
            expected.Equals(actual),
            path,
            $"Actual value {actual} does not match what was expected of {expected} at the given path.");
      }

      private static void AssertIsTrue(bool condition, string path, string message)
      {
         if (!condition)
            throw new JsonAssertFailedException(path, message);
      }

      private static void AssertIsTrue(Func<bool> assertFunction, string path, string message)
      {
         bool result;

         try
         {
            result = assertFunction();
         }
         catch (Exception ex)
         {
            throw new JsonAssertFailedException(path, "Unable to assert value.  " + ex.Message, ex);
         }

         AssertIsTrue(result, path, message);
      }

      private void AssertActualMatchesExpected(object actual, object expected, string path)
      {
         if (expected == null)
            AssertIsTrue(actual == null, path, "Expected null but actual contained a non-null value.");
         else if (expected.GetType().IsValueType)
            AssertObjectsAreEqual(expected, actual, path);
         else if (expected is string)
            AssertStringsAreEqual((string)expected, actual, path);
         else if (expected is object[])
            AssertArraysAreEqual((object[])expected, actual, path);
         else
            AssertDictionariesAreEqual(expected, actual, path);
      }

      private void AssertStringsAreEqual(string expected, object actual, string path)
      {
         var m = Regex.Match(expected, @"^\{(\d*)\}$");

         if (m.Success)
            AssertActualWithAsserter(m.Groups[1].Value, actual, path);
         else
            AssertObjectsAreEqual(expected, actual, path);
      }

      private void AssertActualWithAsserter(string asserter, object actual, string path)
      {
         if (asserter.Length == 0)
            return;

         AssertIsTrue(int.TryParse(asserter, out var i), path, $"Invalid asserter index {asserter} at the given path.");
         AssertIsTrue(i < valueAsserters.Length, path, $"A value asserter [{i}] was not provided.");
         AssertIsTrue(() => valueAsserters[i](actual), path, "Provided value asserter returned false.");
      }

      private void AssertArraysAreEqual(IEnumerable expected, object actual, string path)
      {
         AssertIsTrue(actual is object[], path, "Actual value does not contain a list as expected at the given path.");

         var a = ConvertToType<object[]>(actual, path);
         var e = ConvertToType<object[]>(expected, path);

         AssertActualMatchesExpected(a, e, path);
      }

      private void AssertDictionariesAreEqual(object expected, object actual, string path)
      {
         var a = ConvertToType<IReadOnlyDictionary<string, object>>(actual, path);
         var e = ConvertToType<IReadOnlyDictionary<string, object>>(expected, path);

         AssertActualMatchesExpected(a, e, path);
      }

      private void AssertActualMatchesExpected(object[] actual, object[] expected, string path)
      {
         AssertIsTrue(actual.Length >= expected.Length, path, "Actual value contains a list of fewer items than what is expected at the given path.");

         for (var i = 0; i < expected.Length; i++)
            AssertActualMatchesExpected(actual[i], expected[i], $"{path}[{i}]");
      }

      private void AssertActualMatchesExpected(
         IReadOnlyDictionary<string, object> actual, IReadOnlyDictionary<string, object> expected, string path)
      {
         foreach (var ep in expected)
         {
            var p = $"{path}.{ep.Key}";
            AssertIsTrue(actual.ContainsKey(ep.Key), p, "Actual value does not contain expected property at the given path.");
            AssertActualMatchesExpected(actual[ep.Key], ep.Value, p);
         }
      }

      private T ConvertToType<T>(object o, string path)
      {
         try
         {
            return serializer.ConvertToType<T>(o);
         }
         catch (Exception e)
         {
            throw new JsonAssertFailedException(
               path,
               $"Failed to convert a value to expected type {typeof(T).Name} at the given path.",
               e);
         }
      }
   }
}
