using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gtm.Common.Data;
using Gtm.Common;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.Models.Tests.Services.DesignService
{
   /// <summary>
   /// Fake implementation of the <see cref="IDesignService"/>.
   /// </summary>
   public class FakeDesignService : IDesignService
   {
      private static readonly SizingMeasurements SizingMeasurements = new SizingMeasurements
      {
         Sizes = { "S", "M", "L" },

         Measurements =
         {
            new SizingMeasurements.TypeMeasurements("Chest")
            {
               SizeMeasurements =
               {
                  new SizingMeasurements.SizeMeasurement("S", "34 - 36"),
                  new SizingMeasurements.SizeMeasurement("M", "38 - 40"),
                  new SizingMeasurements.SizeMeasurement("L", "42 - 44")
               }
            }
         }
      };

      private static readonly IDictionary<string, Market> MarketList = new Dictionary<string, Market>
      {
         { "CHH", new Market("CHH", "Cheer", "Cheer") },
         { "CHP", new Market("CHP", "Cheer", "Cheer") }
      };

      private readonly IDictionary<Guid, Design> designList;

      public FakeDesignService()
      {
         designList = new Dictionary<Guid, Design>();
      }

      public Task<Design> FetchDesign(Guid designIdentifier)
      {
         VerifyDesignIdentifier(designIdentifier);

         return Task.FromResult(designList[designIdentifier]);
      }

      public async Task<IList<Design>> RetrieveDesigns(IList<KeyValuePair<string, string>> metadata, IdentifiablePartner partner)
      {
         var identifier = Guid.NewGuid();
         var price = 5.5m;

         var marketCode = GetMetadataValue(metadata, "marketcode");
         var color = GetMetadataValue(metadata, "color");
         var line1 = GetMetadataValue(metadata, "line1");
         var line2 = GetMetadataValue(metadata, "line2");

         if (marketCode != null && !string.IsNullOrWhiteSpace(marketCode) && !MarketList.ContainsKey(marketCode))
            throw new RecordNotFoundException("Market", marketCode);

         if (color != null)
            await FetchColorByName(color);

         if (line1 != null && line2 != null)
            price += 1m;

         var sizes = new Dictionary<string, Size>
         {
            { "S", new Size("S", price) },
            { "M", new Size("M", price) },
            { "L", new Size("L", price) }
         };
         var product = new Product("Hanes Tagless® Short Sleeve Tee", "14500TU", "Light Steel", sizes);
         var design = new Design(identifier, product, GenerateDesignImages(), new VersionInfo(partner.Name), price > 5.5m);

         designList.Add(design.Identifier, design);

         return new List<Design>
         {
            design
         };
      }

      public Task<IList<Color>> RetrieveColors()
      {
         return Task.FromResult<IList<Color>>(new List<Color>
         {
            new Color("Black", "000000"),
            new Color("Silver", "898a8e"),
            new Color("Orange", "e15622")
         });
      }

      public async Task<IList<string>> FetchProductDetails(Guid designIdentifier)
      {
         VerifyDesignIdentifier(designIdentifier);

         return await Task.FromResult<IList<string>>(new List<string>
         {
            "6.0 oz., pre-shrunk 100% ComfortSoft cotton",
            "Shoulder-to-shoulder taping",
            "Tag-free neck label",
            "Double-needle stitched lay-flat collar, armholes and sleeves",
            "CPSIA tracking label compliant"
         });
      }

      public async Task<Color> FetchColorByName(string designColorName)
      {
         var color = (await RetrieveColors()).FirstOrDefault(c => c.Name == designColorName);

         if (color == null)
            throw new ArgumentException("Specified color does not exist. See /color for standard list of colors.", nameof(designColorName));

         return color;
      }

      public Task<SizingMeasurements> FetchSizingMeasurements(Guid designIdentifier)
      {
         VerifyDesignIdentifier(designIdentifier);

         return Task.FromResult(SizingMeasurements);
      }

      public Task<Tuple<string, string[], string[]>> FetchDesignTemplateInformation(Guid designIdentifier)
      {
         throw new NotImplementedException();
      }

      private static string GetMetadataValue(IList<KeyValuePair<string, string>> metadata, string metadataKey)
      {
         string value = null;

         var list = metadata.Where(x => x.Key.ToLower() == metadataKey).ToList();

         if (list.Count > 0)
            return list[0].Value;

         return value;
      }

      private void VerifyDesignIdentifier(Guid designIdentifier)
      {
         ParameterVerifier.VerifyIsNotEmpty(designIdentifier, nameof(designIdentifier));

         if (!designList.ContainsKey(designIdentifier))
            throw new RecordNotFoundException("Design", designIdentifier.ToString("N"));
      }

      private ImageUrls GenerateDesignImages()
      {
         var url = "http://apitest";
         return new ImageUrls(new Uri(url), new Uri(url), new Uri(url), new Uri(url));
      }
   }
}
