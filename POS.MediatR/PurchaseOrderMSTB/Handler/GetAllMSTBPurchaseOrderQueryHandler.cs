using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handler
{
    public class GetAllMSTBPurchaseOrderQueryHandler : IRequestHandler<GetAllMSTBPurchaseOrderQuery, MSTBPurchaseOrderList>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;

        public GetAllMSTBPurchaseOrderQueryHandler(IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
        }

        public async Task<MSTBPurchaseOrderList> Handle(GetAllMSTBPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            return await _mstbPurchaseOrderRepository.GetAllMSTBPurchaseOrders(request.PurchaseOrderResource);
        }
    }
}
