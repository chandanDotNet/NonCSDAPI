using MediatR;
using POS.MediatR.PurchaseOrderMSTB.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class GetMSTBPurchaseOrderItemsReportCommandHandler : IRequestHandler<GetMSTBPurchaseOrderItemsReportCommand, MSTBPurchaseOrderItemList>
    {
        private readonly IMSTBPurchaseOrderItemRepository _mstbpurchaseOrderItemRepository;

        public GetMSTBPurchaseOrderItemsReportCommandHandler(IMSTBPurchaseOrderItemRepository mstbpurchaseOrderItemRepository)
        {
            _mstbpurchaseOrderItemRepository = mstbpurchaseOrderItemRepository;
        }

        public async Task<MSTBPurchaseOrderItemList> Handle(GetMSTBPurchaseOrderItemsReportCommand request, CancellationToken cancellationToken)
        {
            return await _mstbpurchaseOrderItemRepository.GetAllMSTBPurchaseOrderItems(request.PurchaseOrderResource);
        }
    }
}

