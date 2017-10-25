using System.Buffers;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// Allow for pascal formatting when serializing.
   /// </summary>
   public class PascalCaseJsonProfileFormatter : JsonOutputFormatter
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="PascalCaseJsonProfileFormatter"/> class.
      /// </summary>
      public PascalCaseJsonProfileFormatter()
         : base(new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() }, ArrayPool<char>.Shared)
      {
         SupportedMediaTypes.Clear();
         SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json;profile=\"https://en.wikipedia.org/wiki/PascalCase\""));
      }
   }
}
