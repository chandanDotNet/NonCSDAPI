using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class GetNewMSTBPurchaseOrderNumberQueryHandler
        : IRequestHandler<GetNewMSTBPurchaseOrderNumberQuery, string>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;

        public GetNewMSTBPurchaseOrderNumberQueryHandler(IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
        }
        public async Task<string> Handle(GetNewMSTBPurchaseOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var lastMSTBPurchaseOrder = await _mstbPurchaseOrderRepository.All.Where(c => c.IsPurchaseOrderRequest != request.isMSTBPurchaseOrder)
                .OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (lastMSTBPurchaseOrder == null)
            {
                if (request.isMSTBPurchaseOrder)
                {
                    return "GRN#00001";
                }
                else
                {
                    return "PO#00001";
                }
            }

            var lastPONumber = lastMSTBPurchaseOrder.OrderNumber;
            var poId = Regex.Match(lastPONumber, @"\d+").Value;
            var isNumber = int.TryParse(poId, out int poNumber);
            if (isNumber)
            {
                var newPoId = lastPONumber.Replace(poNumber.ToString(), "");
                return $"{newPoId}{poNumber + 1}";
            }
            else
            {
                return $"{lastPONumber}#00001";
            }
        }
    }
}