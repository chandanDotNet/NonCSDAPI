using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Data.Resources;
using POS.MediatR.SalesOrderPayment.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.SalesOrderPayment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesOrderPaymentController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderPaymentController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SalesOrderPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Sales Order payments.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("SO_VIEW_SO_PAYMENTS")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderPaymentDto>))]
        public async Task<IActionResult> GetAllSalesOrderPayments(Guid id)
        {
            var getAllSalesOrderPaymentsCommand = new GetAllSalesOrderPaymentsCommand
            {
                Id = id
            };
            var paymentDtos = await _mediator.Send(getAllSalesOrderPaymentsCommand);

            return Ok(paymentDtos);
        }

        /// <summary>
        /// Create Sales Order payment.
        /// </summary>
        /// <param name="addSalesOrderPaymentCommand"></param>
        /// <returns></returns>
        //[HttpPost, DisableRequestSizeLimit]
        [HttpPost]
        [ClaimCheck("SO_ADD_SO_PAYMENT")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> CreateSalesOrderPayment(AddSalesOrderPaymentCommand addSalesOrderPaymentCommand)
        {
            var result = await _mediator.Send(addSalesOrderPaymentCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Sales Order payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("SO_DELETE_SO_PAYMENT")]
        public async Task<IActionResult> DeleteSalesOrderPayment(Guid id)
        {
            var deleteSalesOrderCommand = new DeleteSalesOrderPaymentCommand
            {
                Id = id
            };
            var response = await _mediator.Send(deleteSalesOrderCommand);
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Get Purchase Order payments Report.
        /// </summary>
        /// <returns></returns>
        [HttpGet("report")]
        [ClaimCheck("REP_SO_PAYMENT_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderPaymentDto>))]
        public async Task<IActionResult> GetAllSalesOrderPaymentsReport([FromQuery] SalesOrderResource salesOrderResource)
        {
            var getAllSalesOrderPaymentsReportCommand = new GetAllSalesOrderPaymentsReportCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var salesOrderPayments = await _mediator.Send(getAllSalesOrderPaymentsReportCommand);

            var paginationMetadata = new
            {
                totalCount = salesOrderPayments.TotalCount,
                pageSize = salesOrderPayments.PageSize,
                skip = salesOrderPayments.Skip,
                totalPages = salesOrderPayments.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(salesOrderPayments);
        }

        /// <summary>
        /// Get Total Purchase Order payments Report.
        /// </summary>
        /// <returns></returns>
        [HttpGet("TotalReport")]
        [ClaimCheck("REP_SO_PAYMENT_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderPaymentDto>))]
        public async Task<IActionResult> GetAllSalesOrderTotalPaymentsReport([FromQuery] SalesOrderResource salesOrderResource)
        {
            TotalPaymentsSalesReportResponseData response=new TotalPaymentsSalesReportResponseData();
            List<PaymentsData> paymentsData = new List<PaymentsData>();
            var getAllSalesOrderPaymentsReportCommand = new GetAllSalesOrderPaymentsReportCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var salesOrderPayments = await _mediator.Send(getAllSalesOrderPaymentsReportCommand);

            //var paginationMetadata = new
            //{
            //    totalCount = salesOrderPayments.TotalCount,
            //    pageSize = salesOrderPayments.PageSize,
            //    skip = salesOrderPayments.Skip,
            //    totalPages = salesOrderPayments.TotalPages
            //};
            
            if (salesOrderPayments!=null)
            {

                var paymentsData1 = salesOrderPayments.GroupBy(x => x.PaymentMethod)
                       .Select(x => new
                       {
                           PaymentMethod = x.Max(y => y.PaymentMethod),                           
                           TotalAmount = x.Sum(y => y.Amount)                         

                       }).ToList();

                foreach(var payments in paymentsData1)
                {
                    var PaymentsData2 = new PaymentsData();
                    PaymentsData2.PaymentMethod = payments.PaymentMethod.ToString();
                    PaymentsData2.TotalAmount = payments.TotalAmount;
                    paymentsData.Add(PaymentsData2);
                }

                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                //response.TotalAmount = salesOrderPayments.Sum(x => x.Amount).ToString("0.00");
                //response.PaymentType = "Cash";
                response.Data = paymentsData;


            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
            }

            

            //Response.Headers.Add("X-Pagination",
            //        Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(response);
        }
    }
}
