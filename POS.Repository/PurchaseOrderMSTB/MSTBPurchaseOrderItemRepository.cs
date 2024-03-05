using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
