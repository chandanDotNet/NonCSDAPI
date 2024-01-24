using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POS.Data.Dto.GRN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class GRN:BaseEntity
    {
        public Guid Id { get; set; }
        public string GRNNumber { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; }
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
        public PaymentStatus PaymentStatus { get; set; }
        public List<GRNItem> GRNItems { get; set; }
       // public List<PurchaseOrderPayment> PurchaseOrderPayments { get; set; }
    }
}
