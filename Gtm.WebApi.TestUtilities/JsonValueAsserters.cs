using System;

namespace Gtm.WebApi.TestUtilities
{
   /// <summary>
   /// Common value asserters to use in verifying JSON values.
   /// </summary>
   public static class JsonValueAsserters
   {
      /// <summary>
      /// Gets the common asserter that ensures a string is not null or empty.
      /// </summary>
      public static Func<object, bool> IsNotNullOrEmpty => v => !string.IsNullOrEmpty((string)v);

      /// <summary>
      /// Common asserter that ensures a <see cref="DateTimeOffset"/> is greater than or equal to a given time.
      /// </summary>
      public static Func<object, bool> IsAtLeast(DateTimeOffset value) => v => DateTimeOffset.Parse((string)v) >= value;
   }
}
