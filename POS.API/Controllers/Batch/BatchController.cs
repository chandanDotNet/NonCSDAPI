using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.MediatR.Batch.Command;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using POS.Data.Resources;
using POS.MediatR;
using Newtonsoft.Json;
using Azure;
using POS.Helper;
using POS.Data;
using POS.Data.Entities;

namespace POS.API.Controllers.Batch
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BatchController : BaseController
    {
        public IMediator _mediator { get; set; }
        public BatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create Batch.
        /// </summary>
        /// <param name="addBatchCommand"></param>
        /// <returns></returns>
        [HttpPost("CreateBatch")]
        [Produces("application/json", "application/xml", Type = typeof(BatchDto))]
        public async Task<IActionResult> AddBatch([FromBody] AddBatchCommand addBatchCommand)
        {
            var result = await _mediator.Send(addBatchCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Batches.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBatches")]
        [Produces("application/json", "application/xml", Type = typeof(List<BatchDto>))]
        public async Task<IActionResult> GetBatches()
        {
            var getAllBatchCommand = new GetAllBatchCommand { };
            var result = await _mediator.Send(getAllBatchCommand);           
            return Ok(result);
        }

    }
}
