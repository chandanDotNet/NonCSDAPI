﻿using Azure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Commands;
using POS.MediatR.SalesOrder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.SalesOrder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesOrderController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SalesOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all sales order.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("SO_VIEW_SALES_ORDERS,REP_SO_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetAllSalesOrder([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getAllSalesOrderQuery = new GetAllSalesOrderCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var salesOrders = await _mediator.Send(getAllSalesOrderQuery);

            var paginationMetadata = new
            {
                totalCount = salesOrders.TotalCount,
                pageSize = salesOrders.PageSize,
                skip = salesOrders.Skip,
                totalPages = salesOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(salesOrders);
        }

        /// <summary>
        /// Get Sales Order.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("SO_VIEW_SO_DETAIL")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetSalesOrder(Guid id)
        {
            var getSalesOrderQuery = new GetSalesOrderCommand
            {
                Id = id
            };
            var salesOrder = await _mediator.Send(getSalesOrderQuery);
            return ReturnFormattedResponse(salesOrder);
        }


        /// <summary>
        /// Creates the sales order.
        /// </summary>
        /// <param name="addSalesOrderCommand">The add sales order command.</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ClaimCheck("SO_ADD_SO,POS_POS")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> CreateSalesOrder(AddSalesOrderCommand addSalesOrderCommand)
        {
            var getNewSalesOrderNumberQuery = new GetNewSalesOrderNumberCommand { };
            var response = await _mediator.Send(getNewSalesOrderNumberQuery);
            if(response!=null)
            {
                addSalesOrderCommand.OrderNumber = response;
            }

            var result = await _mediator.Send(addSalesOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update the Sales order.
        /// </summary>
        /// <param name="updateSalesOrderCommand">The add Sales order command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("SO_UPDATE_SO")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> UpdateSalesOrder(Guid id, UpdateSalesOrderCommand updateSalesOrderCommand)
        {
            var result = await _mediator.Send(updateSalesOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update the Sales order return.
        /// </summary>
        /// <param name="UpdateSalesOrderReturnCommand">The add Sales order command.</param>
        /// <returns></returns>
        [HttpPut("{id}/return")]
        [ClaimCheck("SO_RETURN_SO")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateSalesOrderReturn(Guid id, UpdateSalesOrderReturnCommand updateSalesOrderReturnCommand)
        {
            var result = await _mediator.Send(updateSalesOrderReturnCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Delete Sales Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("SO_DELETE_SO")]
        public async Task<IActionResult> DeleteSalesOrder(Guid id)
        {
            var deleteSalesOrderCommand = new DeleteSalesOrderCommand
            {
                Id = id
            };
            var response = await _mediator.Send(deleteSalesOrderCommand);
            return Ok(response);
        }

        /// <summary>
        /// Gets the new Sales order number.
        /// </summary>
        /// <returns></returns>
        [HttpGet("newOrderNumber")]
        public async Task<IActionResult> GetNewSalesOrderNumber()
        {
            var getNewSalesOrderNumberQuery = new GetNewSalesOrderNumberCommand { };
            var response = await _mediator.Send(getNewSalesOrderNumberQuery);
            return Ok(new
            {
                OrderNumber = response
            });
        }

        /// <summary>
        /// Get Sales order Items
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        [HttpGet("{id}/items")]
        [ClaimCheck("SO_VIEW_SALES_ORDERS")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderItemTaxDto>))]
        public async Task<IActionResult> GetSalesOrderItems(Guid id, bool isReturn = false)
        {
            var getSalesOrderQuery = new GetSalesOrderItemsCommand { Id = id, IsReturn = isReturn };
            var salesOrderItems = await _mediator.Send(getSalesOrderQuery);
            return Ok(salesOrderItems);
        }

        /// <summary>
        /// Get Sales Item report.
        /// </summary>
        /// <param name="salesOrderResource"></param>
        /// <returns></returns>
        [HttpGet("items/reports")]
        [ClaimCheck("SO_VIEW_SALES_ORDERS")]
        public async Task<IActionResult> GetSalesOrderItems([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getSalesOrderItemsReportCommand = new GetSalesOrderItemsReportCommand { SalesOrderResource = salesOrderResource };
            var response = await _mediator.Send(getSalesOrderItemsReportCommand);

            var paginationMetadata = new
            {
                totalCount = response.TotalCount,
                pageSize = response.PageSize,
                skip = response.Skip,
                totalPages = response.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(response);

        }

        /// <summary>
        /// Get Recent Shipment.
        /// </summary>
        /// <returns></returns>
        [HttpGet("recentshipment")]
        [ClaimCheck("DB_RECENT_SO_SHIPMENT")]
        public async Task<IActionResult> GetRecentExpectedShipmentDateSalesOrder()
        {
            var getSalesOrderRecentShipmentDateQuery = new GetSalesOrderRecentShipmentDateQuery { };
            var serviceResponse = await _mediator.Send(getSalesOrderRecentShipmentDateQuery);
            return Ok(serviceResponse);
        }

        /// <summary>
        /// Get Sales order profit and loss Report.
        /// </summary>
        /// <param name="salesOrderResource"></param>
        /// <returns></returns>
        [HttpGet("items/profitLoss")]
        public async Task<IActionResult> GetSalesOrderProfitLossReport([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getSaleOrderProfitLossCommand = new GetSaleOrderProfitLossCommand
            {
                FromDate = salesOrderResource.FromDate.Value,
                ToDate = salesOrderResource.ToDate.Value,
            };

            var response = await _mediator.Send(getSaleOrderProfitLossCommand);
            return Ok(response);
        }

        /// <summary>
        /// Update the Sales order Status.
        /// </summary>
        /// <param name="updateSalesOrderStatusCommand">The add Sales order command.</param>
        /// <returns></returns>
        [HttpPut("UpdateSalesOrderStatus")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> UpdateSalesOrderStatus(UpdateSalesOrderStatusCommand updateSalesOrderStatusCommand)
        {
            var result = await _mediator.Send(updateSalesOrderStatusCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets all sales order summary report.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllSalesOrderSummaryReport")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetAllSalesOrderSummaryReport([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getAllSalesOrderQuery = new GetAllSalesOrderCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var salesOrders = await _mediator.Send(getAllSalesOrderQuery);

            var paginationMetadata = new
            {
                totalCount = salesOrders.TotalCount,
                pageSize = salesOrders.PageSize,
                skip = salesOrders.Skip,
                totalPages = salesOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            //var TotalSO = (from so in salesOrders
            //               select so).Sum(e => e.TotalAmount);

            var Counter = salesOrders.Where(x => x.IsAppOrderRequest == false).GroupBy(x => x.CounterName)
                        .Select(x => new
                        {
                            CounterName = x.Key,
                            TotalAmount = x.Sum(y => y.TotalAmount)
                        }).ToList();

            var App = salesOrders.Where(x => x.IsAppOrderRequest == true).GroupBy(x => x.CounterName)
                       .Select(x => new
                       {
                           CounterName = "App",
                           TotalAmount = x.Sum(y => y.TotalAmount)
                       }).ToList();

            if (App.Count > 0)
            {
                Counter.Insert(Counter.Count, App.FirstOrDefault());
            }
            //else
            //{
            //    Counter = App;
            //}

            //var query = salesOrders
            //            .GroupBy(f => new { f.CounterName })
            //            .Select(group => new { Counter = group.Key, total = group.Sum(f => f.TotalAmount) }).ToList();


            //var lstps = (from e in salesOrders
            //             let cuc_sum = salesOrders.Sum(x => x.TotalAmount)
            //             select new
            //             {
            //                 Counter = e.CounterName,
            //                 TotalAmount = e.TotalAmount
            //             })
            //            .ToList();

            return Ok(Counter);
        }

        /// <summary>
        /// Cancel Sales order Status.
        /// </summary>
        /// <param name="cancelSalesOrderCommand">The add Sales order command.</param>
        /// <returns></returns>
        [HttpPost("CancelSalesOrder")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> CancelSalesOrder([FromBody] CancelSalesOrderCommand cancelSalesOrderCommand)
        {
            var result = await _mediator.Send(cancelSalesOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets all cancel sales order.
        /// </summary>
        /// <returns></returns>
        [HttpGet("CancelSalesOrderList")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetAllCancelSalesOrder([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getAllCancelSalesOrderQuery = new GetAllCancelSalesOrderCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var salesOrders = await _mediator.Send(getAllCancelSalesOrderQuery);

            var paginationMetadata = new
            {
                totalCount = salesOrders.TotalCount,
                pageSize = salesOrders.PageSize,
                skip = salesOrders.Skip,
                totalPages = salesOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(salesOrders);
        }

        /// <summary>
        /// Update the Sales order date time.
        /// </summary>
        /// <param name="updateSalesOrderDateTimeCommand">The add Sales order date time command.</param>
        /// <returns></returns>
        [HttpPut("ChangeSalesOrderDateTime")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> UpdateSalesOrderDateTime(UpdateSalesOrderDateTimeCommand updateSalesOrderDateTimeCommand)
        {
            UpdateSODateTimeResponseData response = new UpdateSODateTimeResponseData();
            var result = await _mediator.Send(updateSalesOrderDateTimeCommand);
            if (result.StatusCode == 200)
            {
                response.status = true;
                response.StatusCode = 200;
                response.message = "Success";                
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";                
            }
            return Ok(response);
        }

    }
}

