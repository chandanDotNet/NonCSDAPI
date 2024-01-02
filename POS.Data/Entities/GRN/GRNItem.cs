using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Data.Entities.GRN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class GRNItem
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; } = PurchaseSaleItemStatusEnum.Not_Return;
        public Guid GRNId { get; set; }
        [ForeignKey("GRNId")]
        public GRN GRN { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal MRP { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalSalesPrice { get; set; }
        public List<GRNItemTax> GRNItemTaxes { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public UnitConversation UnitConversation { get; set; }
        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null;
    }
}
