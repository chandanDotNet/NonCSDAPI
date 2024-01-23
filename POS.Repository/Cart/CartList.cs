using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CartList : List<CartDto>
    {

        public IMapper _mapper { get; set; }
        public CartList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CartList(List<CartDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CartList> Create(IQueryable<Cart> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize=GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CartList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Cart> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public int GetAllData(int totalCount, int pageSize)
        {
            if(pageSize==0)
            {
                pageSize = totalCount;
            }
            return pageSize;
        }

        public async Task<List<CartDto>> GetDtos(IQueryable<Cart> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)              
                .AsNoTracking()
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName,
                    CustomerId = c.CustomerId,
                    ProductId = c.ProductId,
                    ProductName = c.ProductName,
                    UnitId = c.UnitId,
                    UnitName = c.UnitName,
                    UnitPrice = decimal.Parse(c.UnitPrice.ToString("0.00")),
                    Quantity = c.Quantity,
                    Total = decimal.Parse(c.Total.ToString("0.00")),
                    IsAdvanceOrderRequest = c.IsAdvanceOrderRequest == null ? false : Convert.ToBoolean(c.IsAdvanceOrderRequest),
                    Product = _mapper.Map<ProductDto>(c.Product),
                    Inventory = _mapper.Map<InventoryDto>(c.Product.Inventory),
                    PackagingId = c.PackagingId,
                    PackagingName = c.PackagingName

                })
                .ToListAsync();
            foreach (var item in entities.Where(e => e.Product.CategoryId == e.Product.ProductCategory.Id))
            {
                item.Product.CategoryName = item.Product.ProductCategory.Name == null ? "" : item.Product.ProductCategory.Name;
            }
            foreach (var item in entities.Where(e => e.Product.ProductCategory.ProductMainCategoryId == e.Product.ProductCategory.ProductMainCategory.Id))
            {
                item.Product.MainCategoryName = item.Product.ProductCategory.ProductMainCategory.Name == null ? "" : item.Product.ProductCategory.ProductMainCategory.Name;
                item.Product.MainCategoryId = item.Product.ProductCategory.ProductMainCategory.Id;
               
            }
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                if (entity.Inventory != null)
                {
                    entity.Product.Stock = Convert.ToInt64(entity.Product.Inventory.Stock);
                }
                else
                {
                    entity.Product.Stock = 0;
                }
            }
            return entities;
        }

    }
}
