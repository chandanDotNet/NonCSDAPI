using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Helper;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using POS.Data.Entities;

namespace POS.Repository
{
    public class ProductList : List<ProductDto>
    {
        public IMapper _mapper { get; set; }
        public PathHelper _pathHelper { get; set; }
        public ProductList(IMapper mapper, PathHelper pathHelper)
        {
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ProductList(List<ProductDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ProductList> Create(IQueryable<Product> source, int skip, int pageSize, Guid? customerId)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize, customerId.Value);
            var dtoPageList = new ProductList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Product> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public int GetAllData(int totalCount, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = totalCount;
            }
            return pageSize;
        }

        public async Task<List<ProductDto>> GetDtos(IQueryable<Product> source, int skip, int pageSize,Guid? customerId)
        {
            int SNo = skip + 1;
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    SkuName = p.SkuName,
                    SkuCode = p.SkuCode,
                    QRCodeUrl = p.QRCodeUrl,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate,
                    Mrp = p.Mrp,
                    Margin = p.Margin,
                    SalesPrice = p.SalesPrice,
                    PurchasePrice = p.PurchasePrice,
                    CategoryId = p.CategoryId,
                    CategoryName = p.ProductCategory.Name,
                    UnitName = p.Unit.Name,
                    UnitId = p.UnitId,
                    BrandId = p.BrandId,
                    Barcode = p.Barcode,
                    BrandName = p.Brand.Name,
                    WarehouseId = p.WarehouseId,
                    WarehouseName = p.Warehouse.Name,
                    Discount = 0,
                    ProductTaxes = _mapper.Map<List<ProductTaxDto>>(p.ProductTaxes),
                    Unit = _mapper.Map<UnitConversationDto>(p.Unit),
                    ProductUrl = !string.IsNullOrWhiteSpace(p.ProductUrl) ? Path.Combine(_pathHelper.ProductThumbnailImagePath, p.ProductUrl) : "",
                    IsProductOrderTime = p.IsProductOrderTime,
                    OrderStartTime = p.OrderStartTime,
                    OrderEndTime = p.OrderEndTime,
                    RackNo = p.RackNo,
                    HSNCode = p.HSNCode,
                    //Cart= _mapper.Map<CartDto>(c.Cart),
                    Stock = p.Inventory.Stock == null ? 0 : p.Inventory.Stock,
                    IsLoose = p.IsLoose == null ? false : p.IsLoose,
                    MinQty = p.MinQty,
                    PackagingName = p.Packaging.Name != null ? p.Packaging.Name : "",
                    PackagingId = p.Packaging.Id != null ? p.Packaging.Id : new Guid(),
                    ProductTypeId = p.ProductType.Id != null ? p.ProductType.Id : new Guid(),
                    ProductTypeName = p.ProductType.Name != null ? p.ProductType.Name : "",
                    ProductCartQuantity = p.Cart.Where(c => (c.ProductId == p.Id) && c.CustomerId == customerId.Value).FirstOrDefault().Quantity == null ? 0 
                    : p.Cart.Where(c => (c.ProductId == p.Id) && c.CustomerId == customerId.Value).FirstOrDefault().Quantity

                }).ToListAsync();

            entities.ForEach(x => x.SNo = SNo++);
            return entities.ToList();

            //var stockList = entities.Where(x => x.Stock > 0).OrderBy(x => x.Name).ToList();
            //var outstockList = entities.Where(x => x.Stock <= 0).ToList();

            //stockList.AddRange(outstockList);

            //return stockList;
        }
    }
}
