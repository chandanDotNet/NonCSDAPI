using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class MSTBPurchaseOrderItemTax
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderItemId { get; set; }
        [ForeignKey("PurchaseOrderItemId")]
        public MSTBPurchaseOrderItem MSTBPurchaseOrderItem { get; set; }
        public Guid TaxId { get; set; }
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }
    }
}
