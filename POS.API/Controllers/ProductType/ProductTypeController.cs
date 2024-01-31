using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.ProductType.Command;
using POS.MediatR.Packaging.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace POS.API.Controllers.ProductType
{
    public class ProductTypeController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public ProductTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all Packagings
        /// </summary>
        /// <param name="getAllProductTypesQuery"></param>
        /// <returns></returns>       
        [HttpGet("GetProductTypes")]
        [Produces("application/json", "application/xml", Type = typeof(List<ProductTypeDto>))]
        public async Task<IActionResult> GetProductTypes([FromQuery] GetAllProductTypeQuery getAllProductTypesQuery)
        {
            var result = await _mediator.Send(getAllProductTypesQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Packaging.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ProductType/{id}")]
        [Produces("application/json", "application/xml", Type = typeof(ProductTypeDto))]
        public async Task<IActionResult> GetProductType(Guid id)
        {
            var getproductTypeQuery = new GetProductTypeByIdQuery { Id = id };
            var result = await _mediator.Send(getproductTypeQuery);
            return ReturnFormattedResponse(result);
        }
    }
}
