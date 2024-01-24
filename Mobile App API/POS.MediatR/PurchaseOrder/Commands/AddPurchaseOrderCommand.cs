using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using POS.Data;
using Microsoft.AspNetCore.Http;

namespace POS.MediatR.CommandAndQuery
{
    public class AddPurchaseOrderCommand : IRequest<ServiceResponse<PurchaseOrderDto>>
    {
        public string OrderNumber { get; set; }
        public string InvoiceNo { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsPurchaseOrderRequest { get; set; }
        public DateTime POCreatedDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? TotalSaleAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public List<PurchaseOrderItemDto> PurchaseOrderItems { get; set; }
        public string? PurchasePaymentType { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
        public IFormFile? FileDetails { get; set; }
        public string? BatchNo { get; set; }
    }
}
