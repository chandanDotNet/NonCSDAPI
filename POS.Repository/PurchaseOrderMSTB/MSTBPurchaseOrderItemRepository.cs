using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class MSTBPurchaseOrderItemRepository : GenericRepository<MSTBPurchaseOrderItem, POSDbContext>, IMSTBPurchaseOrderItemRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public MSTBPurchaseOrderItemRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<MSTBPurchaseOrderItemList> GetAllMSTBPurchaseOrderItems(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.MSTBPurchaseOrder, c => c.Product).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MSTBPurchaseOrderItemDto, MSTBPurchaseOrderItem>());

            if (purchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Product.SupplierId == purchaseOrderResource.SupplierId);
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.SearchQuery))
            {
                //collectionBeforePaging = collectionBeforePaging
                //    .Where(a => a.Product.Name == purchaseOrderResource.ProductName);

                var genreForWhereClause = purchaseOrderResource.SearchQuery
                   .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");

                collectionBeforePaging = collectionBeforePaging
                   .Where(a => EF.Functions.Like(a.Product.Name, $"%{encodingName}%") || EF.Functions.Like(a.Product.Barcode, $"{encodingName}%"));
            }

            if (purchaseOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProductId == purchaseOrderResource.ProductId);
            }

            if (purchaseOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.POCreatedDate >= new DateTime(purchaseOrderResource.FromDate.Value.Year, purchaseOrderResource.FromDate.Value.Month, purchaseOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (purchaseOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.POCreatedDate <= new DateTime(purchaseOrderResource.ToDate.Value.Year, purchaseOrderResource.ToDate.Value.Month, purchaseOrderResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.SupplierName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.Supplier.SupplierName == purchaseOrderResource.SupplierName.GetUnescapestring());
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.OrderNumber))
            {
                var orderNumber = purchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.MSTBPurchaseOrder.OrderNumber, $"%{orderNumber}%"));
            }

            if (purchaseOrderResource.Year.HasValue && purchaseOrderResource.Month.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.Year == purchaseOrderResource.Year && a.MSTBPurchaseOrder.Month == purchaseOrderResource.Month);
            }

            var purchaseOrders = new MSTBPurchaseOrderItemList();
            return await purchaseOrders
                .Create(collectionBeforePaging, purchaseOrderResource.Skip, purchaseOrderResource.PageSize);
        }
    }
}
