using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;

namespace POS.MediatR.SalesOrder.Commands
{
    public class UpdateSalesOrderReturnCommand: IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsSalesOrderRequest { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public SalesOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public List<SalesOrderItemDto> SalesOrderItems { get; set; }
        public bool? IsAppOrderRequest { get; set; }
        public bool? IsAdvanceOrderRequest { get; set; }

        public bool? OnlineOffline { get; set; }
        public SalesOrderReturnPaymentStatus? PaymentReturnStatus { get; set; }
        public string? UTRNo { get; set; }
        public DateTime? PaymentReturnDate { get; set; }
        public string? OfflineMode { get; set; }
    }
}
