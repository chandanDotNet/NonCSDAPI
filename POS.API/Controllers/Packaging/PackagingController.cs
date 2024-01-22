using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.Packaging.Command;
using POS.MediatR.CommandAndQuery;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace POS.API.Controllers
{
    /// <summary>
    /// Packaging Controller
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class PackagingController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public PackagingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all Packagings
        /// </summary>
        /// <param name="getAllPackagingsQuery"></param>
        /// <returns></returns>       
        [HttpGet("GetPackagings")]
        [Produces("application/json", "application/xml", Type = typeof(List<PackagingDto>))]
        public async Task<IActionResult> GetPackagings([FromQuery] GetAllPackagingQuery getAllPackagingsQuery)
        {
            var result = await _mediator.Send(getAllPackagingsQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Packaging.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Packaging/{id}")]
        [Produces("application/json", "application/xml", Type = typeof(PackagingDto))]
        public async Task<IActionResult> GetPackaging(Guid id)
        {
            var getpackagingQuery = new GetPackagingByIdQuery { Id = id };
            var result = await _mediator.Send(getpackagingQuery);
            return ReturnFormattedResponse(result);
        }
    }
}
