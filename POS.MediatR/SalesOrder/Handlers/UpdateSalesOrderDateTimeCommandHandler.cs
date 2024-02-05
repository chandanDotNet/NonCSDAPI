using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.PurchaseOrder.Handlers;
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
    public class UpdateSalesOrderDateTimeCommandHandler : IRequestHandler<UpdateSalesOrderDateTimeCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSalesOrderDateTimeCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateSalesOrderDateTimeCommandHandler(
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateSalesOrderDateTimeCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }


        public async Task<ServiceResponse<SalesOrderDto>> Handle(UpdateSalesOrderDateTimeCommand request, CancellationToken cancellationToken)
        {
            
            foreach (var SalesOrder in request.SalesOrder)
            {
                var salesOrderExit =  _salesOrderRepository.FindBy(c => c.Id == SalesOrder.Id).FirstOrDefault();
                if(salesOrderExit != null)
                {
                    salesOrderExit.SOCreatedDate = SalesOrder.SOCreatedDate;                    
                    salesOrderExit.DeliveryDate = SalesOrder.DeliveryDate;
                    _salesOrderRepository.Update(salesOrderExit);
                }             

            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Sales Order.");
                return ServiceResponse<SalesOrderDto>.Return500();
            }          
            //var dto = _mapper.Map<SalesOrderDto>(salesOrderExit);
            return ServiceResponse<SalesOrderDto>.ReturnSuccess();
        }

    }
}
