using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class MSTBPurchaseOrder : BaseEntity
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string? InvoiceNo { get; set; }
        public string Note { get; set; }
        public string PurchaseReturnNote { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsPurchaseOrderRequest { get; set; }
        public DateTime POCreatedDate { get; set; }
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Not_Return;
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal? TotalSaleAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<MSTBPurchaseOrderItem> MSTBPurchaseOrderItems { get; set; }
        public List<MSTBPurchaseOrderPayment> MSTBPurchaseOrderPayments { get; set; }
        public string? PurchasePaymentType { get; set; }
        public string? PurchaseOrderPaymentStatus { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
        public string? BatchNo { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool IsMstbGRN { get; set; } = true;
    }
}
