﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.ProductType.Command;
using POS.MediatR.Packaging.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Controllers.ProductType
{
    [Route("api")]
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Searches the Product Type.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpGet("SearchProductTypes", Name = "SearchProductTypes")]
        public async Task<IActionResult> SearchProductType([FromQuery] ProductTypeResource productTypeResource)
        {
            var searchProductTypeQuery = new SearchProductTypeQuery
            {
                ProductTypeResource = productTypeResource
            };
            var result = await _mediator.Send(searchProductTypeQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }
    }
}
