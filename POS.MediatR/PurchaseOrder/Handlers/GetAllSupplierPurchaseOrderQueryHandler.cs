using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllSupplierPurchaseOrderQueryHandler : IRequestHandler<GetAllSupplierPurchaseOrderQuery, PurchaseOrderList>
    {

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetAllSupplierPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrderList> Handle(GetAllSupplierPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderRepository.GetAllPurchaseOrders(request.PurchaseOrderResource);
        }

    }
}
