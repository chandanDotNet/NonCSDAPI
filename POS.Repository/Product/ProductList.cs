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

        public async Task<ProductList> Create(IQueryable<Product> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
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

        public async Task<List<ProductDto>> GetDtos(IQueryable<Product> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new ProductDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    SkuName = c.SkuName,
                    SkuCode = c.SkuCode,
                    QRCodeUrl = c.QRCodeUrl,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                    Mrp = c.Mrp,
                    Margin = c.Margin,
                    SalesPrice = c.SalesPrice,
                    PurchasePrice = c.PurchasePrice,
                    CategoryId = c.CategoryId,
                    CategoryName = c.ProductCategory.Name,
                    UnitName = c.Unit.Name,
                    UnitId = c.UnitId,
                    BrandId = c.BrandId,
                    Barcode = c.Barcode,
                    BrandName = c.Brand.Name,
                    WarehouseId = c.WarehouseId,
                    WarehouseName = c.Warehouse.Name,
                    Discount = 0,
                    ProductTaxes = _mapper.Map<List<ProductTaxDto>>(c.ProductTaxes),
                    Unit = _mapper.Map<UnitConversationDto>(c.Unit),
                    ProductUrl = !string.IsNullOrWhiteSpace(c.ProductUrl) ? Path.Combine(_pathHelper.ProductThumbnailImagePath, c.ProductUrl) : "",
                    IsProductOrderTime = c.IsProductOrderTime,
                    OrderStartTime = c.OrderStartTime,
                    OrderEndTime = c.OrderEndTime,
                    RackNo = c.RackNo,
                    HSNCode = c.HSNCode,
                    //Cart= _mapper.Map<CartDto>(c.Cart),
                    Stock = c.Inventory.Stock == null ? 0 : c.Inventory.Stock,
                    IsLoose = c.IsLoose == null ? false : c.IsLoose,
                    MinQty = c.MinQty,
                    PackagingName = c.Packaging.Name != null ? c.Packaging.Name : "",
                    PackagingId = c.Packaging.Id != null ? c.Packaging.Id : new Guid(),
                    ProductTypeId = c.ProductType.Id != null ? c.ProductType.Id : new Guid(),
                    ProductTypeName = c.ProductType.Name != null ? c.ProductType.Name : ""

                }).ToListAsync();

            return entities.ToList();

            //var stockList = entities.Where(x => x.Stock > 0).OrderBy(x => x.Name).ToList();
            //var outstockList = entities.Where(x => x.Stock <= 0).ToList();

            //stockList.AddRange(outstockList);

            //return stockList;
        }
    }
}
