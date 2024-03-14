using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class MSTBPurchaseOrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; } = PurchaseSaleItemStatusEnum.Not_Return;
        public Guid PurchaseOrderId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public MSTBPurchaseOrder MSTBPurchaseOrder { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<MSTBPurchaseOrderItemTax> MSTBPurchaseOrderItemTaxes { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public UnitConversation UnitConversation { get; set; }
        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null;
        public DateTime? ExpDate { get; set; } = null;
        public decimal? Surplus { get; set; }
        public decimal? Difference { get; set; }
        public bool? IsCheck { get; set; }
        public decimal? NewQuantity { get; set; }
        public decimal? NewMRP { get; set; }
        public bool? IsMRPChanged { get; set; }
        public string Approved { get; set; }        
    }
}
