using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Gtm.Common;
using Gtm.WebApi.TestUtilities.Exceptions;

namespace Gtm.WebApi.TestUtilities
{
   /// <summary>
   /// Provides convenient class for navigating JSON.
   /// </summary>
   /// <remarks>
   /// The usage of this class is for when the caller knows the expected
   /// format of the JSON and provides convenient navigation.  The methods
   /// such as <see cref="P:Item(int)"/>, <see cref="P:Item(string)"/>,
   /// and <see cref="As{T}"/> /// allow for short syntax to get to a particular location.
   /// If the assumption /// made on the format does not hold up,
   /// then <see cref="JsonFormatException"/> is thrown.
   /// </remarks>
   public class JsonValue
   {
      /// <summary>
      /// The constant used to indicate the root of the JSON string.
      /// </summary>
      public const string RootPath = "$";

      /// <summary>
      /// Represents an empty JSON object.
      /// </summary>
      public static JsonValue EmptyValue = new JsonValue("{}");

      private readonly JavaScriptSerializer serializer;
      private readonly object value;
      private readonly string path;

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonValue"/> class.
      /// </summary>
      /// <param name="value">The JSON value.</param>
      public JsonValue(string value)
         : this(value, new JavaScriptSerializer())
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonValue"/> class.
      /// </summary>
      /// <param name="value">The value to be serialized into JSON.</param>
      public JsonValue(object value)
         : this(value, new JavaScriptSerializer())
      {
      }

      private JsonValue(object value, JavaScriptSerializer serializer)
         : this(serializer.Serialize(value), serializer)
      {
      }

      private JsonValue(string value, JavaScriptSerializer serializer)
         : this(serializer.DeserializeObject(value ?? "null"), serializer, RootPath)
      {
      }

      private JsonValue(object value, JavaScriptSerializer serializer, string path)
      {
         this.serializer = serializer;
         this.value = value;
         this.path = path;
      }

      /// <summary>
      /// Gets the path to this value from the original JSON.
      /// </summary>
      public string Path => path;

      /// <summary>
      /// Gets the value as just an <see cref="object"/>.
      /// </summary>
      public object Value => value;

      /// <summary>
      /// Gets a value indicating whether the value is null.
      /// </summary>
      public bool IsNull => value == null;

      /// <summary>
      /// Gets a value indicating whether the value represents an empty object.
      /// </summary>
      public bool IsEmpty => (value as IReadOnlyDictionary<string, object>)?.Count == 0;

      /// <summary>
      /// Indexer for a <see cref="JsonValue"/> when representing a JSON object.
      /// </summary>
      /// <param name="property">Name of the property.</param>
      /// <returns>
      /// The value for the given <paramref name="property"/>.
      /// </returns>
      /// <remarks>
      /// Equivalent to <see cref="GetValue(string)"/> but provided for shorter syntax when chaining.
      /// </remarks>
      /// <seealso cref="GetValue(string)"/>
      public JsonValue this[string property] => GetValue(property);

      /// <summary>
      /// Indexer for a <see cref="JsonValue"/> when representing a JSON array.
      /// </summary>
      /// <param name="index">Index of the desired object.</param>
      /// <returns>
      /// The value at the given <paramref name="index"/>.
      /// </returns>
      /// <remarks>
      /// Equivalent to <see cref="GetObject(int)"/> but provided for shorter syntax when chaining.
      /// </remarks>
      /// <seealso cref="GetObject(int)"/>
      public JsonValue this[int index] => GetObject(index);

      /// <summary>
      /// Allows for implicit conversion from <see cref="JsonValue"/> to a string.
      /// </summary>
      /// <param name="value">The value to convert.</param>
      public static implicit operator string(JsonValue value)
      {
         return value.ToString();
      }

      /// <summary>
      /// Provides method to convert an object to it's JSON string representation.
      /// </summary>
      /// <param name="value">The value for which to generate the JSON.</param>
      /// <returns>
      /// A JSON-formatted <see cref="string"/> of the <paramref name="value"/>.
      /// </returns>
      public static string GenerateJson(object value)
      {
         return new JsonValue(value).ToString();
      }

      /// <summary>
      /// Overrides <see cref="object.ToString()"/> to return JSON representation.
      /// </summary>
      /// <returns>
      /// Returns the value in a JSON representation.
      /// </returns>
      public override string ToString()
      {
         return serializer.Serialize(value);
      }

      /// <summary>
      /// Overrides <see cref="object.GetHashCode()"/> to be calculated
      /// from the JSON string representation.
      /// </summary>
      /// <returns>
      /// A hash code.
      /// </returns>
      public override int GetHashCode()
      {
         return ToString().GetHashCode();
      }

      /// <summary>
      /// Gets the type of the value.
      /// </summary>
      /// <returns>
      /// The type of the value.
      /// </returns>
      public Type GetTypeOfValue()
      {
         return value?.GetType();
      }

      /// <summary>
      /// Provides convenient conversion to a specific type.
      /// </summary>
      /// <typeparam name="T">Target type of the result.</typeparam>
      /// <returns>
      /// Returns the value as type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="JsonFormatException">
      /// Thrown if the value cannot be converted to the target type of <typeparamref name="T"/>.
      /// </exception>
      public T As<T>()
      {
         if (IsEmpty)
            throw new EmptyJsonValueException(path);

         try
         {
            return serializer.ConvertToType<T>(value);
         }
         catch (Exception ex)
         {
            throw new JsonFormatException($"Unable to convert type to {typeof(T).Name}.", path, ex);
         }
      }

      /// <summary>
      /// Returns an instance of <see cref="JsonValue"/> for the given path.
      /// </summary>
      /// <param name="path">The path of the desired value.</param>
      /// <returns>
      /// Returns a value for the given <paramref name="path"/>.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// Thrown if <paramref name="path"/> is <c>null</c>.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// Thrown if <paramref name="path"/> is empty or incorrectly formatted.
      /// </exception>
      /// <exception cref="JsonFormatException">
      /// Thrown if the value cannot be converted to a JSON object or if <paramref name="path"/> does not exist.
      /// </exception>
      /// <remarks>
      /// This method is to be used when you expect this instance to
      /// represent a JSON object.  That is, it is not an array or value, but an object of the form:
      /// <code>
      ///    var v = new JsonValue(@"
      ///       {
      ///          ""Name"":""John Doe"",
      ///          ""Value"":123
      ///       }");
      ///
      ///    Assert.AreEqual(123, v.GetValue("Value")).
      /// </code>
      /// </remarks>
      public JsonValue GetValue(string path)
      {
         ParameterVerifier.VerifyIsNotNullOrEmpty(path, nameof(path));

         if (path.StartsWith(RootPath) && this.path == RootPath)
            return GetValueForPath(path.Substring(RootPath.Length));

         return GetValueForPath(path);
      }

      /// <summary>
      /// Convenient method to return a value for the given property.
      /// </summary>
      /// <typeparam name="T">Type of the value to return.</typeparam>
      /// <param name="property">Name of the property for which to get the value.</param>
      /// <returns>
      /// The value of property <paramref name="property"/> as type <typeparamref name="T"/>.
      /// </returns>
      /// <seealso cref="GetValue(string)"/>
      public T GetValue<T>(string property)
      {
         return GetValue(property).As<T>();
      }

      /// <summary>
      /// Returns an instance of <see cref="JsonValue"/> for the given index of an array.
      /// </summary>
      /// <param name="index">The index of the array.</param>
      /// <returns>
      /// Returns a value for the given <paramref name="index"/>.
      /// </returns>
      /// <exception cref="JsonFormatException">
      /// Thrown if the value cannot be converted to a JSON array.
      /// </exception>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if <paramref name="index"/> exceeds the number of elements within the array.
      /// </exception>
      /// <remarks>
      /// This method is to be used when you expect this instance to
      /// represent a JSON array of objects.  That is, it is not an object or value, but an array of the form:
      /// <code>
      ///    var v = new JsonValue(@"
      ///       [
      ///          {
      ///             ""Name"":""John Doe"",
      ///             ""Value"":1
      ///          },
      ///          {
      ///             ""Name"":""Jim Smith"",
      ///             ""Value"":2
      ///          }
      ///       ]");
      ///
      ///    Assert.IsNotNull(v.GetObject(1)).
      /// </code>
      /// </remarks>
      public JsonValue GetObject(int index)
      {
         var a = As<object[]>();

         if (index >= a.Length)
            throw new ArgumentOutOfRangeException($"Index {index} exceeds the length of the array of {a.Length}.", path);

         return new JsonValue(a[index], serializer, $"{path}[{index}]");
      }

      /// <summary>
      /// Returns a list of <see cref="JsonValue"/> represented by this value.
      /// </summary>
      /// <returns>
      /// Returns a <see cref="IReadOnlyList{T}"/> for the value.
      /// </returns>
      /// <exception cref="JsonFormatException">
      /// Thrown if the value cannot be converted to a JSON array.
      /// </exception>
      /// <remarks>
      /// This method is to be used when you expect this instance to
      /// represent a JSON array of objects.  That is, it is not an object or value, but an array of the form:
      /// <code>
      ///    var v = new JsonValue(@"
      ///       [
      ///          {
      ///             ""Name"":""John Doe"",
      ///             ""Value"":1
      ///          },
      ///          {
      ///             ""Name"":""Jim Smith"",
      ///             ""Value"":2
      ///          }
      ///       ]");
      ///
      ///    Assert.AreEqual(2, v.GetList().Count).
      /// </code>
      /// </remarks>
      public IReadOnlyList<JsonValue> GetList()
      {
         return FindAll(v => true);
      }

      /// <summary>
      /// Returns a list of <see cref="JsonValue"/> represented by this value but with
      /// the given <paramref name="predicate"/> applied.
      /// </summary>
      /// <returns>
      /// Returns a <see cref="IReadOnlyList{T}"/> for the value.
      /// </returns>
      /// <exception cref="JsonFormatException">
      /// Thrown if the value cannot be converted to a JSON array.
      /// </exception>
      /// <remarks>
      /// This method is to be used when you expect this instance to
      /// represent a JSON array of objects.  That is, it is not an object or value, but an array of the form:
      /// <code>
      ///    var v = new JsonValue(@"
      ///       [
      ///          {
      ///             ""Name"":""John Doe"",
      ///             ""Value"":1
      ///          },
      ///          {
      ///             ""Name"":""Jim Smith"",
      ///             ""Value"":2
      ///          }
      ///       ]");
      ///
      ///    Assert.AreEqual(1, v.GetObjects(o => o["Value"] == 2).Count).
      /// </code>
      /// </remarks>
      public IReadOnlyList<JsonValue> FindAll(Predicate<JsonValue> predicate)
      {
         var array = As<object[]>();

         var result = new List<JsonValue>();

         for (var i = 0; i < array.Length; i++)
         {
            var o = new JsonValue(array[i], serializer, $"{path}[{i}]");

            if (predicate(o))
               result.Add(o);
         }

         return result;
      }

      private JsonValue GetValueForPath(string path)
      {
         if (path.Length == 0)
            return this;

         if (path[0] == '.')
            return GetValueForPath(path.Substring(1));

         if (path[0] == '[')
            return GetValueForPathStartingWithArrayIndexer(path);

         return GetValueForPathStartingWithProperty(path);
      }

      private JsonValue GetValueForPathStartingWithArrayIndexer(string path)
      {
         var endingIndex = path.IndexOf(']', 1);

         if (endingIndex <= 1)
            throw new ArgumentException($"Invalid array syntax in path {path}.", nameof(path));

         var indexer = path.Substring(1, endingIndex - 1);

         if (!int.TryParse(indexer, out var n))
            throw new ArgumentException($"Invalid array indexer [{indexer}] in {path}.", nameof(path));

         return GetObject(n).GetValueForPath(path.Substring(endingIndex + 1));
      }

      private JsonValue GetValueForPathStartingWithProperty(string path)
      {
         var p = path.IndexOfAny(new[] { '.', '[' }, 1);

         if (p > 0)
            return GetValueForProperty(path.Substring(0, p)).GetValueForPath(path.Substring(p));

         return GetValueForProperty(path);
      }

      private JsonValue GetValueForProperty(string property)
      {
         var d = As<IReadOnlyDictionary<string, object>>();

         if (!d.ContainsKey(property))
            throw new JsonFormatException($"Property {property} does not exist in object.", path);

         return new JsonValue(d[property], serializer, $"{path}.{property}");
      }
   }
}
