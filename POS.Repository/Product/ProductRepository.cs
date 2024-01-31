using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;

namespace POS.Repository
{
    public class ProductRepository
        : GenericRepository<Product, POSDbContext>, IProductRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public ProductRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            PathHelper pathHelper)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<ProductList> GetProducts(ProductResource productResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Brand, p => p.Packaging, cs => cs.ProductCategory, u => u.Unit, c => c.ProductTaxes, (d => d.Cart), (i => i.Inventory)).OrderBy(p => p.Name).ThenByDescending(p => p.ProductUrl)
               .ApplySort(productResource.ProductUrl, _propertyMappingService.GetPropertyMapping<ProductDto, Product>());

            if (!string.IsNullOrWhiteSpace(productResource.Name))
            {
                // trim & ignore casing
                var genreForWhereClause = productResource.Name
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"%{encodingName}%") || EF.Functions.Like(a.Barcode, $"%{encodingName}%") || EF.Functions.Like(a.Code, $"% {encodingName}%"));
            }

            //if (productResource.CustomerId.HasValue)
            //{
            //    // trim & ignore casing
            //    collectionBeforePaging = collectionBeforePaging
            //       .Where(d => d.Cart.CustomerId == productResource.CustomerId);
            //}

            if (productResource.ProductMainCategoryId.HasValue)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.ProductCategory.ProductMainCategoryId == productResource.ProductMainCategoryId);
            }

            if (!string.IsNullOrWhiteSpace(productResource.Barcode))
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.Barcode == productResource.Barcode);
            }

            if (productResource.UnitId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.UnitId == productResource.UnitId.Value);
            }

            if (productResource.WarehouseId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.WarehouseId == productResource.WarehouseId.Value);
            }
            if (productResource.CategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CategoryId == productResource.CategoryId.Value || a.ProductCategory.ParentId == productResource.CategoryId.Value);
            }

            if (productResource.BrandId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.BrandId == productResource.BrandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(productResource.HSNCode))
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.HSNCode == productResource.HSNCode);
            }
            if (!string.IsNullOrWhiteSpace(productResource.Code))
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.Code == productResource.Code);
            }
            if (productResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == productResource.SupplierId.Value);
            }

            if (productResource.ProductTypeId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProductTypeId == productResource.ProductTypeId.Value && a.Id != productResource.ProductId);
            }

            //collectionBeforePaging = collectionBeforePaging.Where(x => x.Inventory.Stock > 0).OrderBy(x => x.Name);
            //collectionBeforePaging.Where(x => x.Inventory.Stock <= 0)

            var products = new ProductList(_mapper, _pathHelper);
            return await products.Create(collectionBeforePaging, productResource.Skip, productResource.PageSize);
        }
    }
}
