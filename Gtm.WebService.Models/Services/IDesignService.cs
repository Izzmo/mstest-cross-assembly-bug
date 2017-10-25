using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Services.Remoting;
using Gtm.Common.Data;

namespace Gtm.WebService.Models.Services
{
   /// <summary>
   /// The interface for the DesignService.
   /// </summary>
   public interface IDesignService : IService
   {
      /// <summary>
      /// Retrieves a list of designs based on a specific market.
      /// </summary>
      /// <param name="metadata">Key-value list of metadata for design generation.</param>
      /// <param name="partner">A partner</param>
      /// <remarks>If no <paramref name="metadata"/> is given, then a blank design will be generated.</remarks>
      /// <returns>A list of applicable <see cref="Design"/></returns>
      /// <exception cref="ArgumentNullException">Thrown if <paramref name="metadata"/> or <paramref name="partner"/> is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">Thrown if any <paramref name="metadata"/> is invalid.</exception>
      /// <exception cref="RecordNotFoundException">Thrown if marketCode does not exist, if given.</exception>
      Task<IList<Design>> RetrieveDesigns(IList<KeyValuePair<string, string>> metadata, IdentifiablePartner partner);

      /// <summary>
      /// Fetches a design.
      /// </summary>
      /// <param name="designIdentifier">The identifier of the design.</param>
      /// <returns><see cref="Design"/></returns>
      /// <exception cref="ArgumentException">Thrown when <paramref name="designIdentifier"/> is empty.</exception>
      /// <exception cref="RecordNotFoundException">Thrown when <paramref name="designIdentifier"/> is not found.</exception>
      Task<Design> FetchDesign(Guid designIdentifier);

      /// <summary>
      /// Fetches product details.
      /// </summary>
      /// <param name="designIdentifier">The identifier of the design.</param>
      /// <returns> a list of <see cref="string"/></returns>
      /// <exception cref="ArgumentException">Thrown when <paramref name="designIdentifier"/> is empty.</exception>
      /// <exception cref="RecordNotFoundException">Thrown when <paramref name="designIdentifier"/> is not found.</exception>
      Task<IList<string>> FetchProductDetails(Guid designIdentifier);

      /// <summary>
      /// Retrieves template information from a design.
      /// </summary>
      /// <param name="designIdentifier">The identifier of the design.</param>
      /// <returns>
      /// A tuple of 3 items for template information:
      /// <list type="numbered">
      ///   <item>Template name</item>
      ///   <item>Color names</item>
      ///   <item>Text lines</item>
      /// </list>
      /// </returns>
      /// <exception cref="ArgumentException">Thrown when <paramref name="designIdentifier"/> is empty.</exception>
      /// <exception cref="RecordNotFoundException">Thrown when <paramref name="designIdentifier"/> is not found.</exception>
      Task<Tuple<string, string[], string[]>> FetchDesignTemplateInformation(Guid designIdentifier);

      /// <summary>
      /// Retrieves list of colors.
      /// </summary>
      /// <returns>A list of colors.</returns>
      Task<IList<Color>> RetrieveColors();

      /// <summary>
      /// Fetches a color.
      /// </summary>
      /// <param name="colorName">The name of the color to get.</param>
      /// <returns>The requested <see cref="Color"/>.</returns>
      /// <exception cref="ArgumentException">Thrown when <paramref name="colorName"/> does not exist.</exception>
      Task<Color> FetchColorByName(string colorName);

      /// <summary>
      /// Fetches the <see cref="SizingMeasurements"/> for a given design.
      /// </summary>
      /// <param name="designIdentifier">The identifier of the design.</param>
      /// <returns>
      /// The <see cref="SizingMeasurements"/> for the design.
      /// </returns>
      /// <exception cref="ArgumentException">Thrown when <paramref name="designIdentifier"/> is empty.</exception>
      /// <exception cref="RecordNotFoundException">Thrown when <paramref name="designIdentifier"/> is not found.</exception>
      Task<SizingMeasurements> FetchSizingMeasurements(Guid designIdentifier);
   }
}
