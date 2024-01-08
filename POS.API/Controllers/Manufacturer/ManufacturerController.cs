using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.Manufacturer.Command;
using POS.MediatR.CommandAndQuery;
using System.Threading.Tasks;

namespace POS.API.Controllers.Manufacturer
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ManufacturerController : BaseController
    {
        public IMediator _mediator { get; set; }
        public ManufacturerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create Manufacturer.
        /// </summary>
        /// <param name="addManufacturerCommand"></param>
        /// <returns></returns>
        [HttpPost("Manufacturer")]
        [Produces("application/json", "application/xml", Type = typeof(ManufacturerDto))]
        public async Task<IActionResult> AddBrand(AddManufacturerCommand addManufacturerCommand)
        {
            var response = await _mediator.Send(addManufacturerCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Searches the manufacturer.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpGet("SearchManufacturers", Name = "SearchManufacturers")]
        public async Task<IActionResult> SearchManufacturers(string searchQuery, int pageSize)
        {
            var query = new SearchManufacturerQuery
            {
                PageSize = pageSize,
                SearchQuery = searchQuery
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
