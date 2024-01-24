using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto.GRN
{
    public class GRNDto
    {
        public Guid Id { get; set; }
        public string GRNNumber { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; }
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
        public PaymentStatus PaymentStatus { get; set; }
        public List<GRNItemDto> GRNItems { get; set; }
       // public List<PurchaseOrderPaymentDto> PurchaseOrderPayments { get; set; }
        public SupplierDto Supplier { get; set; }
    }
}
