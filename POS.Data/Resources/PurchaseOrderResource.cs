using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class PurchaseOrderResource : ResourceParameter
    {
        public PurchaseOrderResource() : base("POCreatedDate")
        {
        }

        public string OrderNumber { get; set; }
        public string InvoiceNo { get; set; }
        public string SupplierName { get; set; }
        public DateTime? POCreatedDate { get; set; }
        public Guid? SupplierId { get; set; }
        public bool IsPurchaseOrderRequest { get; set; } = false;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.All;
        public string? PurchasePaymentType { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool IsMstbGRN { get; set; }
        public bool IsMSTBGRN { get; set; }
    }
}
