using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderMSTB.Command
{
    public class UpdatePurchaseOrderMSTBCommand : IRequest<ServiceResponse<MSTBPurchaseOrderDto>>
    {
        public Guid Id { get; set; }
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
        public List<MSTBPurchaseOrderItemDto> MSTBPurchaseOrderItems { get; set; }
        public string? PurchasePaymentType { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
        public string? BatchNo { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? Surplus { get; set; }
        public decimal? Difference { get; set; }
        public bool? IsCheck { get; set; }
        public bool IsMstbGRN { get; set; } = true;
    }
}
