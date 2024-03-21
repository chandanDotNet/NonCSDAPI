using POS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class MSTBPurchaseOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public decimal? SalesPrice { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal TaxValue { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string UnitName { get; set; }
        public Guid UnitId { get; set; }
        public decimal Total { get; set; } = 0;
        public DateTime POCreatedDate { get; set; }
        public string SupplierName { get; set; }
        public Guid SupplierId { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime? ExpDate { get; set; }
        public string? Message { get; set; }
        public decimal? Surplus { get; set; }
        public decimal? Difference { get; set; }
        public bool? IsCheck { get; set; }
        public decimal? NewQuantity { get; set; }
        public decimal? NewMRP { get; set; }
        public bool? IsMRPChanged { get; set; }
        public string Approved { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public decimal TotalSaleAmount { get; set; } = 0;
        public UnitConversationDto UnitConversation { get; set; }
        public List<MSTBPurchaseOrderItemTaxDto> MSTBPurchaseOrderItemTaxes { get; set; }
    }
}
