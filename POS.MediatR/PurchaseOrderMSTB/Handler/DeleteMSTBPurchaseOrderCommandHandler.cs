using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class DeleteMSTBPurchaseOrderCommandHandler : IRequestHandler<DeleteMSTBPurchaseOrderCommand, ServiceResponse<bool>>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;
        private readonly IMSTBPurchaseOrderItemRepository _mstbPurchaseOrderItemRepository;
        private readonly ILogger<DeleteMSTBPurchaseOrderCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IInventoryRepository _inventoryRepository;

        public DeleteMSTBPurchaseOrderCommandHandler(IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository,
            ILogger<DeleteMSTBPurchaseOrderCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IInventoryRepository inventoryRepository,
            IMSTBPurchaseOrderItemRepository mstbPurchaseOrderItemRepository)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
            _logger = logger;
            _uow = uow;
            _inventoryRepository = inventoryRepository;
            _mstbPurchaseOrderItemRepository = mstbPurchaseOrderItemRepository;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteMSTBPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _mstbPurchaseOrderRepository.AllIncluding(c => c.MSTBPurchaseOrderItems).FirstOrDefaultAsync(c => c.Id == request.Id);

            if (purchaseOrder == null)
            {
                _logger.LogError("Purchase order does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            purchaseOrder.IsDeleted = true;
            _mstbPurchaseOrderRepository.Update(purchaseOrder);

            _mstbPurchaseOrderItemRepository.RemoveRange(purchaseOrder.MSTBPurchaseOrderItems);

            //var inventories = purchaseOrder.MSTBPurchaseOrderItems
            //      .Select(cs => new InventoryDto
            //      {
            //          ProductId = cs.ProductId,
            //          PricePerUnit = cs.UnitPrice,
            //          PurchaseOrderId = purchaseOrder.Id,
            //          Stock = cs.Status == PurchaseSaleItemStatusEnum.Not_Return ? cs.Quantity : (-1) * cs.Quantity,
            //          UnitId = cs.UnitId,
            //          WarehouseId = cs.WarehouseId,
            //          TaxValue = cs.TaxValue,
            //          Discount = cs.Discount,
            //      }).ToList();



            //inventories.ForEach(invetory =>
            //{
            //    invetory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(invetory);
            //});

            //var inventoriesToDelete = inventories
            //    .GroupBy(c => c.ProductId)
            //    .Select(cs => new InventoryDto
            //    {
            //        InventorySource = Data.InventorySourceEnum.DeletePurchaseOrder,
            //        ProductId = cs.Key,
            //        PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
            //        PurchaseOrderId = purchaseOrder.Id,
            //        Stock = cs.Sum(d => d.Stock),
            //    }).ToList();

            //foreach (var inventory in inventoriesToDelete)
            //{
            //    await _inventoryRepository.AddInventory(inventory);
            //}

            //var warehouseInventoriesToAdd = inventories
            //    .Where(c => c.WarehouseId.HasValue)
            //    .GroupBy(c => new { c.WarehouseId, c.ProductId })
            //    .Select(cs => new InventoryDto
            //    {
            //        InventorySource = InventorySourceEnum.DeletePurchaseOrder,
            //        ProductId = cs.Key.ProductId,
            //        Stock = cs.Sum(d => d.Stock),
            //        WarehouseId = cs.Key.WarehouseId
            //    }).ToList();

            //foreach (var inventory in warehouseInventoriesToAdd)
            //{
            //    await _inventoryRepository.AddWarehouseInventory(inventory);
            //}

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting Purchase order.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
