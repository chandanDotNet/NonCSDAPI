using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class InventoryList : List<InventoryDto>
    {
        public InventoryList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public InventoryList(List<InventoryDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<InventoryList> Create(IQueryable<Inventory> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new InventoryList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Inventory> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<InventoryDto>> GetDtos(IQueryable<Inventory> source, int skip, int pageSize)
        {

            if (pageSize == 0)
            {
                var entities = await source
            .AsNoTracking()
            .Select(c => new InventoryDto
            {
                Id = c.Id,
                AveragePurchasePrice = c.AveragePurchasePrice,
                AverageSalesPrice = c.AverageSalesPrice,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                SalePrice = c.Product.SalesPrice,
                PurchasePrice = c.Product.PurchasePrice,
                Mrp = c.Product.Mrp,
                Margin = c.Product.Margin,
                Barcode = c.Product.Barcode,
                ProductCode = c.Product.Code,
                ProductCategoryName = c.Product.ProductCategory.Name,
                ManufacturerName = c.Product.Manufacturer.ManufacturerName,
                Stock = c.Stock,
                UnitName = c.Product.Unit.Name,
                UnitId = c.Product.Unit.Id,
                BrandName = c.Product.Brand.Name,
                BrandId = c.Product.Brand.Id,
                SupplierName = c.Product.PurchaseOrderItems.PurchaseOrder.Supplier.SupplierName,
                SupplierId = c.Product.PurchaseOrderItems.PurchaseOrder.Supplier.Id,
                TotalStockAmount = c.AveragePurchasePrice * c.Stock
            }).ToListAsync();
                return entities.DistinctBy(x => x.ProductId).ToList();
            }
            else
            {
                var entities = await source
               .Skip(skip)
               .Take(pageSize)
               .AsNoTracking()
               .Select(c => new InventoryDto
               {
                   Id = c.Id,
                   AveragePurchasePrice = c.AveragePurchasePrice,
                   AverageSalesPrice = c.AverageSalesPrice,
                   ProductId = c.ProductId,
                   ProductName = c.Product.Name,
                   SalePrice = c.Product.SalesPrice,
                   PurchasePrice = c.Product.PurchasePrice,
                   Mrp = c.Product.Mrp,
                   Margin = c.Product.Margin,
                   Barcode = c.Product.Barcode,
                   ProductCode = c.Product.Code,
                   ProductCategoryName = c.Product.ProductCategory.Name,
                   ManufacturerName = c.Product.Manufacturer.ManufacturerName,
                   Stock = c.Stock,
                   UnitName = c.Product.Unit.Name,
                   UnitId = c.Product.Unit.Id,
                   BrandName = c.Product.Brand.Name,
                   BrandId = c.Product.Brand.Id,
                   SupplierName = c.Product.PurchaseOrderItems.PurchaseOrder.Supplier.SupplierName,
                   SupplierId = c.Product.PurchaseOrderItems.PurchaseOrder.Supplier.Id,
                   TotalStockAmount = c.AveragePurchasePrice * c.Stock
               }).ToListAsync();
                return entities.DistinctBy(x => x.ProductId).ToList();
            }

        }
    }
}
