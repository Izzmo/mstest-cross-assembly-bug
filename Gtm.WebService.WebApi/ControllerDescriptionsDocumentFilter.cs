using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gtm.WebService.WebApi
{
   /// <summary>
   /// Adds Controller Descriptions to Swagger Documents.
   /// </summary>
   public class ControllerDescriptionsDocumentFilter : IDocumentFilter
   {
      /// <summary>
      /// Apply descriptions.
      /// </summary>
      public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
      {
         swaggerDoc.Tags = new[]
         {
            new Tag { Name = "Color", Description = "The resources available for a customer to choose their organization's colors." },
            new Tag { Name = "Customer", Description = "The resource for the partner's customer." },
            new Tag { Name = "Design", Description = "A design is a specific combination of product and art." },
            new Tag { Name = "Market", Description = " A market can be a sport, activity, or an event that is relevant to a customer." },
            new Tag { Name = "Order", Description = "Submit orders to GTM." },
            new Tag { Name = "OrderSummary", Description = "Used to send and validate cart and checkout monetary totals for subtotal, sales tax, and shipping" }
         };
      }
   }
}
