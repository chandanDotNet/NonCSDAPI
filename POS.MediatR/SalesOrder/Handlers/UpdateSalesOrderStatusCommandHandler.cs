using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using POS.MediatR.Brand.Command;
using POS.MediatR.Brand.Handler;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace POS.MediatR.SalesOrder.Handlers
{
    public class UpdateSalesOrderStatusCommandHandler : IRequestHandler<UpdateSalesOrderStatusCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateSalesOrderStatusCommand> _logger;
        private readonly IMapper _mapper;
        public UpdateSalesOrderStatusCommandHandler(
           ISalesOrderRepository salesOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateSalesOrderStatusCommand> logger,           
            IMapper mapper
            )
        {
            _salesOrderRepository = salesOrderRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<SalesOrderDto>> Handle(UpdateSalesOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _salesOrderRepository.FindBy(c => c.Id == request.SalesOrderId)
             .FirstOrDefaultAsync();

            if (!entityExist.UserId.HasValue)
            {
                entityExist.UserId = request.DeliveryPersonId;
                //entityExist.OrderDeliveryStatus = "In-Process";
            }

            else
            {
                entityExist.OrderDeliveryStatus = request.OrderDeliveryStatus;
            }  
            
            _salesOrderRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<SalesOrderDto>.Return500();
            }            
            var result = _mapper.Map<SalesOrderDto>(entityExist);            
            return ServiceResponse<SalesOrderDto>.ReturnResultWith200(result);
        }
    }
}