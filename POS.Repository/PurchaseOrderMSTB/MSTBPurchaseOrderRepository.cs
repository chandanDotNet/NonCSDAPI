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
    public class MSTBPurchaseOrderRepository : GenericRepository<MSTBPurchaseOrder, POSDbContext>, IMSTBPurchaseOrderRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public MSTBPurchaseOrderRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<MSTBPurchaseOrderList> GetAllMSTBPurchaseOrders(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Supplier, p => p.MSTBPurchaseOrderItems).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<PurchaseOrderDto, PurchaseOrder>());

            //if (!string.IsNullOrEmpty(purchaseOrderResource.SearchQuery))
            //{
            //    // trim & ignore casing
            //    var genreForWhereClause = purchaseOrderResource.SearchQuery.ToString();
            //    var name = Uri.UnescapeDataString(genreForWhereClause);
            //    var encodingName = WebUtility.UrlDecode(name);
            //    var ecapestring = Regex.Unescape(encodingName);
            //    encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
            //    collectionBeforePaging = collectionBeforePaging
            //        //.Where(a => EF.Functions.Like(a.MSTBPurchaseOrderItems.ToString(), $"{encodingName}%"));
            //        .Where(a => a.MSTBPurchaseOrderItems.Any(c => EF.Functions.Like(c.ProductId.ToString(), $"{encodingName}%")));
            //}

            if (purchaseOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MSTBPurchaseOrderItems.Any(c => c.ProductId == purchaseOrderResource.ProductId));
            }

            if (!string.IsNullOrEmpty(purchaseOrderResource.OrderNumber))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.OrderNumber == purchaseOrderResource.OrderNumber);
            }

            if (purchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == purchaseOrderResource.SupplierId);
            }

            if (purchaseOrderResource.Year.HasValue && purchaseOrderResource.Month.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Year == purchaseOrderResource.Year && a.Month == purchaseOrderResource.Month);
            }
           
            var purchaseOrders = new MSTBPurchaseOrderList();
            return await purchaseOrders
                .Create(collectionBeforePaging, purchaseOrderResource.Skip, purchaseOrderResource.PageSize);
        }

        public async Task<MSTBPurchaseOrderList> GetAllMSTBPurchaseOrdersReport(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Supplier, c => c.MSTBPurchaseOrderItems).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MSTBPurchaseOrderDto, MSTBPurchaseOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsPurchaseOrderRequest == purchaseOrderResource.IsPurchaseOrderRequest);

            if (purchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == purchaseOrderResource.SupplierId);
            }



            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.SupplierName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Supplier.SupplierName == purchaseOrderResource.SupplierName.GetUnescapestring());
            }

            if (purchaseOrderResource.POCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate == purchaseOrderResource.POCreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.OrderNumber))
            {
                var orderNumber = purchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));
            }


            var purchaseOrders = new MSTBPurchaseOrderList();
            return await purchaseOrders
                .Create(collectionBeforePaging, 0, 0);
        }

    }
}