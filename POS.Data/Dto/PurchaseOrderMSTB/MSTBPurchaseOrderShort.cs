using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class MSTBPurchaseOrderShort
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderName { get; set; }
        public int Quantity { get; set; }
        public string SupplierName { get; set; }
        public Guid SupplierId { get; set; }
    }
}
