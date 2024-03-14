using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers.PurchaseOrderMSTB;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrderMSTB.Command;
using System.Threading.Tasks;
using System;
using POS.Data.Resources;
using System.Collections.Generic;
using POS.Data;
using POS.Data.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using POS.MediatR.Inventory.Command;
using System.Linq;
using POS.MediatR.Commands;
using POS.MediatR.CustomerAddress.Commands;

namespace POS.API.Controllers.PurchaseOrderMSTB
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderMSTBController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseOrderMSTBController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public PurchaseOrderMSTBController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the purchase order MSTB.
        /// </summary>
        /// <param name="addPurchaseOrderMSTBCommand">The add purchase order command.</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(MSTBPurchaseOrderDto))]
        public async Task<IActionResult> CreatePurchaseOrderMSTB(AddPurchaseOrderMSTBCommand addPurchaseOrderMSTBCommand)
        {
            InventoryResource inventoryResource = new InventoryResource();
            inventoryResource.SupplierId = addPurchaseOrderMSTBCommand.SupplierId;
            inventoryResource.Month = addPurchaseOrderMSTBCommand.Month - 1;
            inventoryResource.Year = addPurchaseOrderMSTBCommand.Year;
            inventoryResource.PageSize = 0;
            inventoryResource.Skip = 0;

            var getAllInventoryCommand = new GetAllInventoryCommand
            {
                InventoryResource = inventoryResource,
            };
            var resultInventory = await _mediator.Send(getAllInventoryCommand);

            //Json Data start

            //Header
            addPurchaseOrderMSTBCommand.Month = addPurchaseOrderMSTBCommand.Month;
            addPurchaseOrderMSTBCommand.PurchasePaymentType = "Credit";
            addPurchaseOrderMSTBCommand.DeliveryDate = DateTime.Now;
            addPurchaseOrderMSTBCommand.DeliveryStatus = DeliveryStatus.Completely_Delivery;
            addPurchaseOrderMSTBCommand.IsPurchaseOrderRequest = false;
            addPurchaseOrderMSTBCommand.POCreatedDate = DateTime.Now;
            addPurchaseOrderMSTBCommand.BatchNo = "1";
            addPurchaseOrderMSTBCommand.TotalAmount = resultInventory.Where(x => x.ClosingStock > 0).Sum(x => x.PurchasePrice.Value);
            addPurchaseOrderMSTBCommand.TotalSaleAmount = resultInventory.Where(x => x.ClosingStock > 0).Sum(x => x.SalePrice);

            //Item            
            foreach (var item in resultInventory)
            {
                addPurchaseOrderMSTBCommand.MSTBPurchaseOrderItems.Add(new MSTBPurchaseOrderItemDto()
                {
                    Surplus = 0,
                    Difference = 0,
                    Discount = 0,
                    DiscountPercentage = 0,
                    ProductId = item.ProductId,
                    UnitId = item.UnitId,
                    WarehouseId = item.WarehouseId,
                    Quantity = item.ClosingStock,
                    Margin = item.Margin,
                    SalesPrice = item.SalePrice,
                    Mrp = item.Mrp,
                    UnitPrice = item.PurchasePrice.Value,
                    ProductCode = item.ProductCode,
                    IsCheck = false,
                    IsMRPChanged = false,
                    NewMRP = item.Mrp,
                    NewQuantity = item.ClosingStock,
                    Approved = string.Empty,
                    MSTBPurchaseOrderItemTaxes = new List<MSTBPurchaseOrderItemTaxDto>()
                });
            }

            //Json Data End

            PurchaseOrderMSTBData responseData = new PurchaseOrderMSTBData();
            var getNewMSTBPurchaseOrderNumberQuery = new GetNewMSTBPurchaseOrderNumberQuery
            {
                isMSTBPurchaseOrder = true
            };
            var response = await _mediator.Send(getNewMSTBPurchaseOrderNumberQuery);

            if (response != null)
            {
                addPurchaseOrderMSTBCommand.OrderNumber = response;
            }
            var result = await _mediator.Send(addPurchaseOrderMSTBCommand);

            if (result.Success)
            {
                POMSTB obj = new POMSTB()
                {
                    Id = result.Data.Id,
                    OrderNumber = result.Data.OrderNumber,
                    Month = result.Data.Month.Value,
                    Year = result.Data.Year.Value,
                    SupplierId = result.Data.SupplierId,
                };

                responseData.status = true;
                responseData.StatusCode = 1;
                responseData.message = "success";
                responseData.Data = obj;
            }
            else
            {
                responseData.status = false;
                responseData.StatusCode = 0;
                responseData.message = "failed.";
            }

            return Ok(responseData);
            //return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update the purchase order MSTB.
        /// </summary>
        /// <param name="updatePurchaseOrderMSTBCommand">The add purchase order command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(MSTBPurchaseOrderDto))]
        public async Task<IActionResult> UpdatePurchaseOrderMSTB(Guid id, UpdatePurchaseOrderMSTBCommand updatePurchaseOrderMSTBCommand)
        {
            PurchaseOrderMSTBData responseData = new PurchaseOrderMSTBData();
            var result = await _mediator.Send(updatePurchaseOrderMSTBCommand);
            //return ReturnFormattedResponse(result);

            if (result.Success)
            {
                POMSTB obj = new POMSTB()
                {
                    Id = result.Data.Id,
                    OrderNumber = result.Data.OrderNumber,
                    Month = result.Data.Month.Value,
                    Year = result.Data.Year.Value,
                    SupplierId = result.Data.SupplierId,
                };

                responseData.status = true;
                responseData.StatusCode = 1;
                responseData.message = "success";
                responseData.Data = obj;
            }
            else
            {
                responseData.status = false;
                responseData.StatusCode = 0;
                responseData.message = "failed.";
            }

            return Ok(responseData);
        }

        /// <summary>
        /// Gets all purchase order by Supplier.
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllPurchaseOrderBySupplier")]
        [Produces("application/json", "application/xml", Type = typeof(List<MSTBPurchaseOrderDto>))]
        public async Task<IActionResult> GetAllPurchaseOrderBySupplier([FromBody] PurchaseOrderResource purchaseOrderResource)
        {
            var getAllMSTBPurchaseOrderQuery = new GetAllMSTBPurchaseOrderQuery
            {
                PurchaseOrderResource = purchaseOrderResource
            };
            var purchaseOrders = await _mediator.Send(getAllMSTBPurchaseOrderQuery);

            var paginationMetadata = new
            {
                totalCount = purchaseOrders.TotalCount,
                pageSize = purchaseOrders.PageSize,
                skip = purchaseOrders.Skip,
                totalPages = purchaseOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(purchaseOrders);
        }

        /// <summary>
        /// Gets the new MSTB purchase order number.
        /// </summary>
        /// <returns></returns>
        [HttpGet("newMSTBOrderNumber/{isMSTBPurchaseOrder}")]
        public async Task<IActionResult> GetNewMSTBPurchaseOrderNumber(bool isMSTBPurchaseOrder)
        {
            var getNewMSTBPurchaseOrderNumberQuery = new GetNewMSTBPurchaseOrderNumberQuery
            {
                isMSTBPurchaseOrder = isMSTBPurchaseOrder
            };
            var response = await _mediator.Send(getNewMSTBPurchaseOrderNumberQuery);
            return Ok(new
            {
                OrderNumber = response
            });
        }


        [HttpGet("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(List<MSTBPurchaseOrderDto>))]
        public async Task<IActionResult> GetPurchaseOrderMSTB(Guid id)
        {
            var getMSTBPurchaseOrderQuery = new GetMSTBPurchaseOrderQuery
            {
                Id = id
            };
            var mstbPurchaseOrder = await _mediator.Send(getMSTBPurchaseOrderQuery);
            NoDataResponse response = new NoDataResponse();
            if (mstbPurchaseOrder.Data == null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "success";
                response.Data = new string[0];
                return Ok(response);
            }
            return ReturnFormattedResponse(mstbPurchaseOrder);
        }

        [HttpPost("GetAllPurchaseOrderMSTBReviewer")]
        [Produces("application/json", "application/xml", Type = typeof(List<MSTBPurchaseOrderDto>))]
        public async Task<IActionResult> GetPurchaseOrderMSTBReviewer([FromBody] PurchaseOrderResource purchaseOrderResource)
        {
            var getMSTBReviewerPurchaseOrderQuery = new GetMSTBReviewerPurchaseOrderQuery
            {
                SupplierId = purchaseOrderResource.SupplierId.Value,
                Month = purchaseOrderResource.Month.Value,
                Year = purchaseOrderResource.Year.Value,
                //ProductName = purchaseOrderResource.ProductName

            };
            var mstbReviewerPurchaseOrder = await _mediator.Send(getMSTBReviewerPurchaseOrderQuery);
            NoDataResponse response = new NoDataResponse();
            if (mstbReviewerPurchaseOrder.Data == null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "success";
                response.Data = new string[0];
                return Ok(response);
            }

            return ReturnFormattedResponse(mstbReviewerPurchaseOrder);
        }

        /// <summary>
        /// Delete Purchase Order MSTB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMSTBPurchaseOrder(Guid id)
        {
            var deletePurchaseOrderCommand = new DeleteMSTBPurchaseOrderCommand
            {
                Id = id
            };
            var response = await _mediator.Send(deletePurchaseOrderCommand);
            return Ok(response);
        }

        /// <summary>
        /// Get MSTB Purchase Order Items.
        /// </summary>
        /// <param name="mstbpurchaseOrderResource"></param>
        /// <returns></returns>
        [HttpPost("mstbitems/mstbreports")]
        public async Task<IActionResult> GetMSTBPurchaseOrderItems([FromBody] PurchaseOrderResource mstbpurchaseOrderResource)
        {
            var getmstbPurchaseOrderItemsCommand = new GetMSTBPurchaseOrderItemsReportCommand { PurchaseOrderResource = mstbpurchaseOrderResource };
            var result = await _mediator.Send(getmstbPurchaseOrderItemsCommand);

            //var paginationMetadata = new
            //{
            //    totalCount = response.TotalCount,
            //    pageSize = response.PageSize,
            //    skip = response.Skip,
            //    totalPages = response.TotalPages
            //};

            //Response.Headers.Add("X-Pagination",
            //    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            //return Ok(response);

            MSTBPurchaseOrderItemResponseData response = new MSTBPurchaseOrderItemResponseData();
            if (result != null)
            {
                response.Done = result.Where(x => x.IsCheck == true).Count();
                response.Pending = result.Where(x => x.IsCheck == false).Count(); ;
                response.status = true;
                response.StatusCode = 1;
                response.message = "success";
                response.Data = result;
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "failed";
            }
            return Ok(response);

        }


        /// <summary>
        /// Update MSTB Purchase Order Item.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatePurchaseOrderItemMSTBCommand"></param>
        /// <returns></returns>
        [HttpPut("PurchaseOrderItemMSTB")]
        [Produces("application/json", "application/xml", Type = typeof(MSTBPurchaseOrderItemDto))]
        public async Task<IActionResult> UpdatePurchaseOrderItemMSTB(UpdatePurchaseOrderItemMSTBCommand updatePurchaseOrderItemMSTBCommand)
        {
            var result = await _mediator.Send(updatePurchaseOrderItemMSTBCommand);

            //return ReturnFormattedResponse(result);

            NoDataResponse response = new NoDataResponse();
            if (result != null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "success";
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "failed";
            }
            return Ok(response);
        }
    }
}
