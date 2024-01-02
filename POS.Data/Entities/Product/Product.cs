using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class Product : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string Description { get; set; }
        public string ProductUrl { get; set; }
        public string QRCodeUrl { get; set; }
        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public UnitConversation Unit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalesPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<ProductTax> ProductTaxes { get; set; }
        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }
        public Cart Cart { get; set; }
        public bool? IsProductOrderTime { get; set; }
        public string? OrderStartTime { get; set; }
        public string? OrderEndTime { get; set; }
        public string? RackNo { get; set; }
        public Inventory Inventory { get; set; }
        public string? HSNCode { get; set; }
        public PurchaseOrderItem PurchaseOrderItems { get; set; }

    }
}
