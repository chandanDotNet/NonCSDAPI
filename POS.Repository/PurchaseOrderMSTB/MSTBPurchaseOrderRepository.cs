using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}