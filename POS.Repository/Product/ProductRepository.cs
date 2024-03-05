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
        private readonly IInventoryRepository _iInventoryRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public ProductRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            PathHelper pathHelper,
            IInventoryRepository iInventoryRepository)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _pathHelper = pathHelper;
            _iInventoryRepository = iInventoryRepository;
        }

        public async Task<ProductList> GetProducts(ProductResource productResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Brand, p => p.Packaging, cs => cs.ProductCategory, u => u.Unit, c => c.ProductTaxes, (d => d.Cart))
                //.Where(i => i.Inventory.Month == DateTime.Now.Month && i.Inventory.Year == DateTime.Now.Year)
                .OrderBy(p => p.ProductUrl == null ? 1 : 0).ThenBy(p => p.Name)
                .ApplySort(productResource.ProductUrl, _propertyMappingService.GetPropertyMapping<ProductDto, Product>());

            //collectionBeforePaging = collectionBeforePaging.OrderBy(x => x.Name).ThenByDescending(x => x.Inventory.Stock != null ? x.Inventory.Stock : 0);

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
                    .Where(a => EF.Functions.Like(a.Name, $"%{encodingName}%") || EF.Functions.Like(a.Barcode, $"{encodingName}%") || EF.Functions.Like(a.Code, $"{encodingName}%"));
            }


            if (!string.IsNullOrWhiteSpace(productResource.SupplierName))
            {
                // trim & ignore casing
                var genreForWhereClause = productResource.SupplierName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Supplier.SupplierName, $"%{encodingName}%"));
            }

            //if (productResource.CustomerId.HasValue)
            //{
            //    // trim & ignore casing
            //    collectionBeforePaging = collectionBeforePaging
            //       .Where(d => d.Cart.CustomerId == productResource.CustomerId);
            //}
            if (productResource.Id.HasValue)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.Id == productResource.Id);
            }
            if (productResource.ProductMainCategoryId.HasValue)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.ProductCategory.ProductMainCategoryId == productResource.ProductMainCategoryId);
            }
            if (productResource.IsAppOrderRequest == true)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging.
                    Where(x => x.Brand.Name != "Baggage" && x.Brand.Name != "DELIVERY");
            }

            //if (productResource.ProductMainCategoryId.HasValue)
            //{
            //    // trim & ignore casing
            //    collectionBeforePaging = collectionBeforePaging
            //       .Where(a => a.ProductCategory.ProductMainCategoryId == productResource.ProductMainCategoryId);
            //}

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

            if (productResource.BrandNameFilter != null && productResource.BrandNameFilter.Length > 0)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => productResource.BrandNameFilter.Contains(a.BrandId));
            }

            if (productResource.PriceLesser.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesPrice >= productResource.PriceLesser.Value && a.SalesPrice <= productResource.PriceGreater.Value);
            }

            if (productResource.PriceLesser.HasValue || productResource.PriceGreater.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesPrice >= productResource.PriceLesser.Value && a.SalesPrice <= productResource.PriceGreater.Value);
            }

            if (productResource.PriceHighToLow == true)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging.OrderByDescending(x => x.SalesPrice);
            }

            if (productResource.PriceLowToHigh == true)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging.OrderBy(x => x.SalesPrice);
            }

            if (productResource.AlphabaticalOrder == true)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging.OrderBy(x => x.Name);
            }

            //collectionBeforePaging = collectionBeforePaging.Where(x => x.Inventory.Stock > 0).OrderBy(x => x.Name);
            //collectionBeforePaging.Where(x => x.Inventory.Stock <= 0)

            var products = new ProductList(_mapper, _pathHelper, _iInventoryRepository);
            return await products.Create(collectionBeforePaging, productResource.Skip, productResource.PageSize, productResource.CustomerId);
        }
    }
}
