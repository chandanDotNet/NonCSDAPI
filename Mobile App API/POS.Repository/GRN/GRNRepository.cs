using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Data.Dto.GRN;

namespace POS.Repository
{
    public class GRNRepository :GenericRepository<GRN, POSDbContext>, IGRNRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public GRNRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }


        public async Task<GRNList> GetAllGRN(GRNResource gRNResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Supplier).ApplySort(gRNResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<GRNDto, GRN>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsPurchaseOrderRequest == gRNResource.IsPurchaseOrderRequest);

            if (gRNResource.Status != PurchaseOrderStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == gRNResource.Status);
            }

            if (gRNResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == gRNResource.SupplierId);
            }

            if (gRNResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.GRNItems.Any(c => c.ProductId == gRNResource.ProductId));
            }

            if (gRNResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate >= new DateTime(gRNResource.FromDate.Value.Year, gRNResource.FromDate.Value.Month, gRNResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (gRNResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate <= new DateTime(gRNResource.ToDate.Value.Year, gRNResource.ToDate.Value.Month, gRNResource.ToDate.Value.Day, 23, 59, 59));
            }

            //if (!string.IsNullOrWhiteSpace(gRNResource.SupplierName))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.Supplier.SupplierName == gRNResource.SupplierName.GetUnescapestring());
            //}

            if (gRNResource.POCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate == gRNResource.POCreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(gRNResource.GRNNumber))
            {
                var gRNNumber = gRNResource.GRNNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.GRNNumber, $"%{gRNNumber}%"));
            }


            var purchaseOrders = new GRNList();
            return await purchaseOrders
                .Create(collectionBeforePaging, gRNResource.Skip, gRNResource.PageSize);
        }

    }
}
