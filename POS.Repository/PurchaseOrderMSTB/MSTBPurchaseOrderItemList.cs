using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class MSTBPurchaseOrderItemList : List<MSTBPurchaseOrderItemDto>
    {
        public MSTBPurchaseOrderItemList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public MSTBPurchaseOrderItemList(List<MSTBPurchaseOrderItemDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<MSTBPurchaseOrderItemList> Create(IQueryable<MSTBPurchaseOrderItem> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new MSTBPurchaseOrderItemList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<MSTBPurchaseOrderItem> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<MSTBPurchaseOrderItemDto>> GetDtos(IQueryable<MSTBPurchaseOrderItem> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
             .AsNoTracking()
             .Select(c => new MSTBPurchaseOrderItemDto
             {
                 ProductName = c.Product.Name,
                 ProductCode= c.Product.Barcode,
                 UnitName = c.Product.Unit.Name,
                 UnitPrice = c.UnitPrice,
                 Margin = c.Margin,
                 Mrp = c.Mrp,
                 SalesPrice = c.SalesPrice,
                 Quantity = c.Quantity,
                 DiscountPercentage = c.DiscountPercentage,
                 Discount = c.Discount,
                 TaxValue = c.TaxValue,
                 ProductId = c.ProductId,
                 WarehouseId = c.WarehouseId,
                 WarehouseName = c.Warehouse.Name,
                 PurchaseOrderId = c.PurchaseOrderId,
                 PurchaseOrderNumber = c.MSTBPurchaseOrder.OrderNumber,
                 SupplierName = c.MSTBPurchaseOrder.Supplier.SupplierName,
                 SupplierId = c.MSTBPurchaseOrder.Supplier.Id,
                 POCreatedDate = c.MSTBPurchaseOrder.POCreatedDate,
                 Status = c.Status,
                 ExpDate = c.ExpDate,
                 Id = c.Id,
                 IsCheck = c.IsCheck,
                 Difference = c.Difference,
                 Surplus = c.Surplus,
                 UnitId = c.UnitId,
                 NewQuantity = c.NewQuantity,
                 NewMRP = c.NewMRP,
                 IsMRPChanged = c.IsMRPChanged.Value,
                 Approved = c.Approved,
                 Total = (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,                 
                 MSTBPurchaseOrderItemTaxes = c.MSTBPurchaseOrderItemTaxes.Select(c => new MSTBPurchaseOrderItemTaxDto
                 {
                     TaxName = c.Tax.Name,
                     TaxPercentage = c.Tax.Percentage,
                 }).ToList()
             })
             .ToListAsync();
                return entities;
            }
            else
            {

                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(c => new MSTBPurchaseOrderItemDto
             {
                 ProductName = c.Product.Name,
                 ProductCode = c.Product.Barcode,
                 UnitName = c.Product.Unit.Name,
                 UnitPrice = c.UnitPrice,
                 Margin = c.Margin,
                 Mrp = c.Mrp,
                 SalesPrice = c.SalesPrice,
                 Quantity = c.Quantity,
                 DiscountPercentage = c.DiscountPercentage,
                 Discount = c.Discount,
                 TaxValue = c.TaxValue,
                 ProductId = c.ProductId,
                 PurchaseOrderId = c.PurchaseOrderId,
                 POCreatedDate = c.MSTBPurchaseOrder.POCreatedDate,
                 PurchaseOrderNumber = c.MSTBPurchaseOrder.OrderNumber,
                 SupplierName = c.MSTBPurchaseOrder.Supplier.SupplierName,
                 SupplierId = c.MSTBPurchaseOrder.Supplier.Id,
                 Status = c.Status,
                 WarehouseId = c.WarehouseId,
                 WarehouseName = c.Warehouse.Name,
                 ExpDate = c.ExpDate,
                 Id = c.Id,
                 IsCheck = c.IsCheck,
                 Difference = c.Difference,
                 Surplus = c.Surplus,
                 UnitId = c.UnitId,
                 NewQuantity = c.NewQuantity,
                 NewMRP = c.NewMRP,
                 IsMRPChanged = c.IsMRPChanged.Value,
                 Approved = c.Approved,
                 Total = (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,
                 MSTBPurchaseOrderItemTaxes = c.MSTBPurchaseOrderItemTaxes.Select(c => new MSTBPurchaseOrderItemTaxDto
                 {
                     TaxName = c.Tax.Name,
                     TaxPercentage = c.Tax.Percentage,
                 }).ToList()
             })
             .ToListAsync();
                return entities;
            }

        }
    }
}
