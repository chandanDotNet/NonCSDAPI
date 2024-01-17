using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Data.Entities;

namespace POS.Repository
{
    public class InventoryList : List<InventoryDto>
    {
        public IMapper _mapper { get; set; }
        public InventoryList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public DateTime? DefaultDate { get; private set; }

        public InventoryList(List<InventoryDto> items, int count, int skip, int pageSize, DateTime? defaultDate)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            DefaultDate = defaultDate;
            AddRange(items);
        }

        public async Task<InventoryList> Create(IQueryable<Inventory> source, int skip, int pageSize, DateTime? defaultDate)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize, defaultDate);
            var dtoPageList = new InventoryList(dtoList, count, skip, pageSize, defaultDate);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Inventory> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<InventoryDto>> GetDtos(IQueryable<Inventory> source, int skip, int pageSize, DateTime? defaultDate)
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

                //SupplierName = c.Product.,
                // SupplierId = c.Product.SupplierId,
                TotalStockAmount = c.AveragePurchasePrice * c.Stock,
                ClosingStock = c.Stock,
                InventoryHistories = _mapper.Map<List<InventoryHistoryDto>>(c.Product.InventoryHistories),
                //OpeningStock = c.Product.InventoryHistories.Where(cs => (cs.ProductId == c.ProductId)
                //&& cs.CreatedDate >= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 0, 0, 1)
                //&& cs.CreatedDate <= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 23, 59, 59))
                //.OrderByDescending(cs => cs.CreatedDate)
                //.FirstOrDefault().Stock

            }).ToListAsync();
                //return entities.DistinctBy(x => x.ProductId).ToList();
                entities = entities.DistinctBy(x => x.ProductId).ToList();

                for (int i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    if (entity.InventoryHistories != null)
                    {
                        var openingStock = entities[i].InventoryHistories.Where(x => (x.ProductId == entities[i].ProductId)
                        && x.CreatedDate >= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 0, 0, 1)
                        && x.CreatedDate <= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 23, 59, 59))
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();
                        if (openingStock != null)
                        {
                            entities[i].OpeningStock = openingStock.PreviousTotalStock;
                        }
                    }
                }

                return entities;
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
                   TotalStockAmount = c.AveragePurchasePrice * c.Stock,
                   ClosingStock = c.Stock,
                   InventoryHistories = _mapper.Map<List<InventoryHistoryDto>>(c.Product.InventoryHistories),
                   //OpeningStock = c.Product.InventoryHistories.Where(cs => (cs.ProductId == c.ProductId)
                   //&& cs.CreatedDate >= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 0, 0, 1)
                   //&& cs.CreatedDate <= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 23, 59, 59))
                   //.OrderByDescending(cs => cs.CreatedDate)
                   //.FirstOrDefault().Stock

               }).ToListAsync();
                //return entities.DistinctBy(x => x.ProductId).ToList();

                entities = entities.DistinctBy(x => x.ProductId).ToList();

                for (int i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    if (entity.InventoryHistories != null)
                    {
                        var openingStock = entity.InventoryHistories.Where(x => (x.ProductId == entity.ProductId)
                        && x.CreatedDate >= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 0, 0, 1)
                        && x.CreatedDate <= new DateTime(defaultDate.Value.Year, defaultDate.Value.Month, defaultDate.Value.Day, 23, 59, 59))
                        .OrderBy(x => x.CreatedDate)
                        .FirstOrDefault();
                        if (openingStock != null)
                        {
                            entities[i].OpeningStock = openingStock.PreviousTotalStock;
                        }
                    }
                }

                return entities;
            }

        }
    }
}
