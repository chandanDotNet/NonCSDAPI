using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Handlers
{
    public class GetAllCancelSalesOrderQuaryHandler : IRequestHandler<GetAllCancelSalesOrderCommand, SalesOrderList>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        public GetAllCancelSalesOrderQuaryHandler(ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }
        public Task<SalesOrderList> Handle(GetAllCancelSalesOrderCommand request, CancellationToken cancellationToken)
        {
            return _salesOrderRepository.GetAllCancelSalesOrders(request.SalesOrderResource);
        }
    }
}
