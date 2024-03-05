using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class MSTBPurchaseOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string? InvoiceNo { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsPurchaseOrderRequest { get; set; }
        public DateTime POCreatedDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal? TotalSaleAmount { get; set; }
        public decimal? TotalReturnAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<MSTBPurchaseOrderItemDto> MSTBPurchaseOrderItems { get; set; }
        public List<MSTBPurchaseOrderPaymentDto> MSTBPurchaseOrderPayments { get; set; }
        public SupplierDto Supplier { get; set; }
        public string? PurchasePaymentType { get; set; }
        public string? PurchaseOrderPaymentStatus { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
        public string? BatchNo { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool IsMstbGRN { get; set; } = true;
    }
}
