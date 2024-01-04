using AutoMapper;
using MediatR;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Inventory.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using POS.MediatR.PurchaseOrder.Handlers;

namespace POS.MediatR.Inventory.Handler
{
    public class CleanInventoryCommandHandler : IRequestHandler<CleanInventoryCommand, ServiceResponse<bool>>
    {
        private readonly IWarehouseInventoryRepository _warehouseInventoryRepository;
        private readonly IInventoryHistoryRepository _inventoryHistoryRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<CleanInventoryCommandHandler> _logger;

        public CleanInventoryCommandHandler(IInventoryRepository inventoryRepository,            
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<CleanInventoryCommandHandler> logger,
            IInventoryHistoryRepository inventoryHistoryRepository,
            IWarehouseInventoryRepository warehouseInventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _inventoryHistoryRepository = inventoryHistoryRepository;
            _warehouseInventoryRepository = warehouseInventoryRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(CleanInventoryCommand request, CancellationToken cancellationToken)
        {
            var warehouseInventoryEntity = _warehouseInventoryRepository.All.ToList();
            var inventoryHistoryEntity = _inventoryHistoryRepository.All.ToList();
            var inventoryEntity = _inventoryRepository.All.ToList();            
            if (warehouseInventoryEntity != null || inventoryHistoryEntity != null || inventoryEntity != null)
            {
                _warehouseInventoryRepository.RemoveRange(warehouseInventoryEntity);
                _inventoryHistoryRepository.RemoveRange(inventoryHistoryEntity);
                _inventoryRepository.RemoveRange(inventoryEntity);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while deleting");
                    return ServiceResponse<bool>.Return500();
                }
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            return ServiceResponse<bool>.Return404();
        }
    }
}
