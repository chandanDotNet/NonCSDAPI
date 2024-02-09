using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ProductDto
    {
        public int SNo { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string Description { get; set; }
        public string ProductUrl { get; set; }
        public string QRCodeUrl { get; set; }
        public Guid UnitId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
        public Guid BrandId { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string BrandName { get; set; }
        public decimal? Discount { get; set; }
        public UnitConversationDto Unit { get; set; }
        public List<ProductTaxDto> ProductTaxes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public CartDto Cart { get; set; }
        public bool? IsProductOrderTime { get; set; }
        public string? OrderStartTime { get; set; }
        public string? OrderEndTime { get; set; }
        public string? RackNo { get; set; }
        //public ProductCategoryDto ProductCategory { get; set; }
        public InventoryDto Inventory { get; set; }
        public decimal? Stock { get; set; }
        public string? HSNCode { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string MainCategoryName { get; set; }
        public Guid? MainCategoryId { get; set; }
        public bool? IsLoose { get; set; }
        public decimal? MinQty { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public SupplierDto Supplier { get; set; }
        public string PackagingName { get; set; }
        public Guid? PackagingId { get; set; }
        public Guid? ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public decimal? ProductCartQuantity { get; set; }
    }
}
