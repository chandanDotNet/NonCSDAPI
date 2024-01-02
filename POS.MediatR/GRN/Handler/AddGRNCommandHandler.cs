using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Dto.GRN;
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

namespace POS.MediatR.GRN.Handler
{
    public class AddGRNCommandHandler : IRequestHandler<AddGRNCommand, ServiceResponse<GRNDto>>
    {

        private readonly IGRNRepository _gRNRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddGRNCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public AddGRNCommandHandler(
            IGRNRepository gRNRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddGRNCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _gRNRepository = gRNRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResponse<GRNDto>> Handle(AddGRNCommand request, CancellationToken cancellationToken)
        {

            var existingPONumber = _gRNRepository.All.Any(c => c.GRNNumber == request.GRNNumber);
            if (existingPONumber)
            {
                return ServiceResponse<GRNDto>.Return409("Purchase Order Number is already Exists.");
            }

            var purchaseOrder = _mapper.Map<Data.GRN>(request);
            purchaseOrder.PaymentStatus = PaymentStatus.Pending;
            purchaseOrder.GRNItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.GRNItemTaxes.ForEach(tax => { tax.Tax = null; });
                item.CreatedDate = DateTime.UtcNow;
            });
            _gRNRepository.Add(purchaseOrder);

            if (!request.IsPurchaseOrderRequest)
            {
                var inventories = purchaseOrder.GRNItems
                   .Select(cs => new InventoryDto
                   {
                       ProductId = cs.ProductId,
                       PricePerUnit = cs.UnitPrice,
                       PurchaseOrderId = purchaseOrder.Id,
                       Stock = cs.Quantity,
                       UnitId = cs.UnitId,
                       WarehouseId = cs.WarehouseId,
                       TaxValue = cs.TaxValue,
                       Discount = cs.Discount
                   }).ToList();

                inventories.ForEach(invetory =>
                {
                    invetory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(invetory);
                });

                var inventoriesToAdd = inventories
                    .GroupBy(c => c.ProductId)
                    .Select(cs => new InventoryDto
                    {
                        InventorySource = InventorySourceEnum.PurchaseOrder,
                        ProductId = cs.Key,
                        PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                        PurchaseOrderId = purchaseOrder.Id,
                        Stock = cs.Sum(d => d.Stock),
                    }).ToList();

                foreach (var inventory in inventoriesToAdd)
                {
                    await _inventoryRepository.AddInventory(inventory);
                }

                var warehouseInventoriesToAdd = inventories
                    .Where(c => c.WarehouseId.HasValue)
                    .GroupBy(c => new { c.WarehouseId, c.ProductId })
                    .Select(cs => new InventoryDto
                    {
                        InventorySource = InventorySourceEnum.PurchaseOrder,
                        ProductId = cs.Key.ProductId,
                        Stock = cs.Sum(d => d.Stock),
                        WarehouseId = cs.Key.WarehouseId
                    }).ToList();

                foreach (var inventory in warehouseInventoriesToAdd)
                {
                    await _inventoryRepository.AddWarehouseInventory(inventory);
                }
            }
            //var aa = await _uow.SaveAsync();

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order.");
                return ServiceResponse<GRNDto>.Return500();
            }
            var dto = _mapper.Map<GRNDto>(purchaseOrder);
            return ServiceResponse<GRNDto>.ReturnResultWith201(dto);
        }


    }
}
