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
        public async Task<MSTBPurchaseOrderItemList> GetAllMSTBPurchaseOrderItems(MSTBPurchaseOrderResource mstbpurchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.MSTBPurchaseOrder, c => c.Product).ApplySort(mstbpurchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MSTBPurchaseOrderItemDto, MSTBPurchaseOrderItem>());

            if (mstbpurchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Product.SupplierId == mstbpurchaseOrderResource.SupplierId);
            }

            if (!string.IsNullOrWhiteSpace(mstbpurchaseOrderResource.SearchQuery))
            {
                //collectionBeforePaging = collectionBeforePaging
                //    .Where(a => a.Product.Name == purchaseOrderResource.ProductName);

                var genreForWhereClause = mstbpurchaseOrderResource.SearchQuery
                   .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");

                collectionBeforePaging = collectionBeforePaging
                   .Where(a => EF.Functions.Like(a.Product.Name, $"%{encodingName}%") || EF.Functions.Like(a.Product.Barcode, $"{encodingName}%"));
            }

            if (mstbpurchaseOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProductId == mstbpurchaseOrderResource.ProductId);
            }

            if (mstbpurchaseOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.POCreatedDate >= new DateTime(mstbpurchaseOrderResource.FromDate.Value.Year, mstbpurchaseOrderResource.FromDate.Value.Month, mstbpurchaseOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (mstbpurchaseOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.POCreatedDate <= new DateTime(mstbpurchaseOrderResource.ToDate.Value.Year, mstbpurchaseOrderResource.ToDate.Value.Month, mstbpurchaseOrderResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(mstbpurchaseOrderResource.SupplierName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.Supplier.SupplierName == mstbpurchaseOrderResource.SupplierName.GetUnescapestring());
            }

            if (!string.IsNullOrWhiteSpace(mstbpurchaseOrderResource.OrderNumber))
            {
                var orderNumber = mstbpurchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.MSTBPurchaseOrder.OrderNumber, $"%{orderNumber}%"));
            }

            if (mstbpurchaseOrderResource.Year.HasValue && mstbpurchaseOrderResource.Month.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrder.Year == mstbpurchaseOrderResource.Year && a.MSTBPurchaseOrder.Month == mstbpurchaseOrderResource.Month);
            }

            var purchaseOrders = new MSTBPurchaseOrderItemList();
            return await purchaseOrders
                .Create(collectionBeforePaging, mstbpurchaseOrderResource.Skip, mstbpurchaseOrderResource.PageSize);
        }
    }
}
