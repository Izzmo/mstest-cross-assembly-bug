using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// Proxy class for <see cref="IDesignService"/>
   /// </summary>
   public class DesignServiceProxy : StatefulServiceProxy<IDesignService>, IDesignService
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="DesignServiceProxy"/> class.
      /// </summary>
      public DesignServiceProxy()
         : base(new Uri("fabric:/Gtm.WebService/DesignService"))
      {
      }

      /// <summary>
      /// See <see cref="IDesignService"/>
      /// </summary>
      public Task<Design> FetchDesign(Guid designIdentifier)
      {
         return InvokeService(() => Service.FetchDesign(designIdentifier));
      }

      /// <summary>
      /// See <see cref="IDesignService"/>
      /// </summary>
      public Task<IList<Design>> RetrieveDesigns(IList<KeyValuePair<string, string>> metadata, IdentifiablePartner partner)
      {
         return InvokeService(() => Service.RetrieveDesigns(metadata, partner));
      }

      /// <summary>
      /// See <see cref="IDesignService"/>
      /// </summary>
      /// <returns></returns>
      public Task<IList<Color>> RetrieveColors()
      {
         return InvokeService(() => Service.RetrieveColors());
      }

      /// <summary>
      /// <see cref="IDesignService.FetchColorByName(string)"/>
      /// </summary>
      public Task<Color> FetchColorByName(string colorName)
      {
         return InvokeService(() => Service.FetchColorByName(colorName));
      }

      /// <summary>
      /// <see cref="IDesignService.FetchDesignTemplateInformation(Guid)"/>
      /// </summary>
      public Task<Tuple<string, string[], string[]>> FetchDesignTemplateInformation(Guid designIdentifier)
      {
         return InvokeService(() => Service.FetchDesignTemplateInformation(designIdentifier));
      }

      /// <summary>
      /// <see cref="IDesignService.FetchProductDetails(Guid)"/>
      /// </summary>
      public Task<IList<string>> FetchProductDetails(Guid designIdentifier)
      {
         return InvokeService(() => Service.FetchProductDetails(designIdentifier));
      }

      /// <summary>
      /// <see cref="IDesignService.FetchSizingMeasurements"/>
      /// </summary>
      public Task<SizingMeasurements> FetchSizingMeasurements(Guid designIdentifier)
      {
         return InvokeService(() => Service.FetchSizingMeasurements(designIdentifier));
      }
   }
}
