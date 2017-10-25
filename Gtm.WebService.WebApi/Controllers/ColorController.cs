using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Gtm.WebService.Models;
using Gtm.WebService.Models.Services;

namespace Gtm.WebService.WebApi.Controllers
{
   /// <summary>
   /// Color Controller
   /// </summary>
   [Route("color"), Authorize]
   public class ColorController : WebApiController
   {
      private readonly IDesignService designService;

      /// <summary>
      /// Initializes a new instance of the <see cref="ColorController"/> class.
      /// </summary>
      /// <param name="designService">The design microservice proxy.</param>
      public ColorController(IDesignService designService)
      {
         this.designService = designService;
      }

      /// <summary>
      /// Gets GTM's basic color palette.
      /// </summary>
      /// <returns>List of colors</returns>
      [HttpGet, Route("")]
      [ProducesResponseType(200, Type = typeof(Color[]))]
      [SwaggerResponse(200, typeof(Color[]))]
      [SwaggerResponse(401, typeof(string), "User not authorized.")]
      public async Task<IActionResult> GetColors()
      {
         return Ok(await designService.RetrieveColors());
      }
   }
}
