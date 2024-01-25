using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Domain;
using POS.Helper;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class UpdateSalesOrderCommandHandler
        : IRequestHandler<UpdateSalesOrderCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSalesOrderCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateSalesOrderCommandHandler(
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateSalesOrderCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResponse<SalesOrderDto>> Handle(UpdateSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var existingSONumber = _salesOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber && c.Id != request.Id);
            if (existingSONumber)
            {
                return ServiceResponse<SalesOrderDto>.Return409("Sales Order Number is already Exists.");
            }

            var salesOrderExit = await _salesOrderRepository.FindAsync(request.Id);
            if (salesOrderExit.Status == Data.SalesOrderStatus.Return)
            {
                return ServiceResponse<SalesOrderDto>.Return409("Sales Order can't edit becuase it's already Return.");
            }

            var salesOrderItemsExist = await _salesOrderItemRepository.FindBy(c => c.SalesOrderId == request.Id).ToListAsync();

            // remove existing warehouse inventory
            var inventoriesToRemove = salesOrderItemsExist
                .Where(c => c.WarehouseId.HasValue)
                .Select(cs => new InventoryDto
                {
                    ProductId = cs.ProductId,
                    PricePerUnit = cs.UnitPrice,
                    SalesOrderId = salesOrderExit.Id,
                    Stock = cs.Quantity,
                    UnitId = cs.UnitId,
                    WarehouseId = cs.WarehouseId,
                    TaxValue = cs.TaxValue,
                    Discount = cs.Discount
                }).ToList();

            inventoriesToRemove.ForEach(invetory =>
            {
                invetory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(invetory);
            });

            var warehouseInventoriesToRemove = inventoriesToRemove
                 .GroupBy(c => new { c.WarehouseId, c.ProductId })
                 .Select(cs => new InventoryDto
                 {
                     InventorySource = InventorySourceEnum.SalesOrder,
                     ProductId = cs.Key.ProductId,
                     Stock = cs.Sum(d => d.Stock),
                     WarehouseId = cs.Key.WarehouseId
                 }).ToList();

            if (warehouseInventoriesToRemove.Any())
            {
                await _inventoryRepository.RemoveExistingWareHouseInventory(warehouseInventoriesToRemove);
                if (await _uow.SaveAsync() <= 0)
                {
                    //_logger.LogError("Error while Updating Sales Order.");
                    //return ServiceResponse<SalesOrderDto>.Return500();
                }
            }

            _salesOrderItemRepository.RemoveRange(salesOrderItemsExist);

            var salesOrderUpdate = _mapper.Map<POS.Data.SalesOrder>(request);
            salesOrderUpdate.SalesOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.SalesOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
            });

            salesOrderExit.OrderNumber = salesOrderUpdate.OrderNumber;
            salesOrderExit.CustomerId = salesOrderUpdate.CustomerId;
            salesOrderExit.Note = salesOrderUpdate.Note;
            salesOrderExit.TermAndCondition = salesOrderUpdate.TermAndCondition;
            salesOrderExit.IsSalesOrderRequest = salesOrderUpdate.IsSalesOrderRequest;
            if (salesOrderExit.IsAdvanceOrderRequest == true)
            {
                salesOrderExit.SOCreatedDate = salesOrderUpdate.DeliveryDate;
            }
            else
            {
                salesOrderExit.SOCreatedDate = salesOrderUpdate.SOCreatedDate;
            }
            salesOrderExit.SOCreatedDate = salesOrderUpdate.SOCreatedDate;
            salesOrderExit.Status = salesOrderUpdate.Status;
            salesOrderExit.DeliveryDate = salesOrderUpdate.DeliveryDate;
            salesOrderExit.DeliveryStatus = salesOrderUpdate.DeliveryStatus;
            salesOrderExit.CustomerId = salesOrderUpdate.CustomerId;
            salesOrderExit.TotalAmount = salesOrderUpdate.TotalAmount;
            salesOrderExit.TotalTax = salesOrderUpdate.TotalTax;
            salesOrderExit.TotalDiscount = salesOrderUpdate.TotalDiscount;
            salesOrderExit.SalesOrderItems = salesOrderUpdate.SalesOrderItems;
            //salesOrderExit.SalesOrderItems.ForEach(c =>
            //{
            //    c.SalesOrderId = salesOrderUpdate.Id;
            //    c.CreatedDate = DateTime.UtcNow;
            //    //c.TotalSalesPrice= decimal.Round((decimal)c.UnitPrice*c.Quantity);
            //    decimal value = (decimal)(c.UnitPrice) * c.Quantity;
            //    int roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
            //    c.TotalSalesPrice = (decimal)roundedValue;
            //});


            //==================

            if (salesOrderExit.SalesOrderItems.Count > 0)
            {
                for (int i = 0; i < salesOrderExit.SalesOrderItems.Count; i++)
                {
                    decimal TotalPurPrice = 0, value = 0;
                    int roundedValue = 0;
                    //var product = _productRepository.All.Where(c => c.Id == salesOrder.SalesOrderItems[i].ProductId).FirstOrDefault();
                    //if (product != null)
                    //{
                    //    TotalPurPrice = Math.Round((decimal)product.PurchasePrice) * salesOrder.SalesOrderItems[i].Quantity;
                    //}
                    salesOrderExit.SalesOrderItems[i].SalesOrderId = salesOrderUpdate.Id;
                    value = (decimal)(salesOrderExit.SalesOrderItems[i].UnitPrice) * salesOrderExit.SalesOrderItems[i].Quantity;
                    roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    salesOrderExit.SalesOrderItems[i].TotalSalesPrice = roundedValue;
                    //salesOrderExit.SalesOrderItems[i].TotalPurPrice = TotalPurPrice;
                    salesOrderExit.SalesOrderItems[i].CreatedDate = DateTime.Now;
                    //entity.SalesOrderItems[i].TotalSalesPrice = decimal.Round((decimal)(entity.SalesOrderItems[i].UnitPrice * entity.SalesOrderItems[i].Quantity));
                    //_logger.LogError("TotalSalesPrice -", roundedValue);
                }
            }

            //==================

            decimal TotalAmount = 0;
            TotalAmount = (decimal)salesOrderExit.SalesOrderItems.Sum(item => item.TotalSalesPrice);
            salesOrderExit.TotalAmount = (decimal)Math.Round(TotalAmount, MidpointRounding.AwayFromZero);

            //salesOrderExit.TotalAmount = decimal.Round((decimal)salesOrderExit.SalesOrderItems.Sum(item => item.TotalSalesPrice));
            _salesOrderRepository.Update(salesOrderExit);

            var inventories = request.SalesOrderItems
                .Select(cs => new InventoryDto
                {
                    ProductId = cs.ProductId,
                    PricePerUnit = cs.UnitPrice,
                    SalesOrderId = salesOrderExit.Id,
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
                    InventorySource = InventorySourceEnum.SalesOrder,
                    ProductId = cs.Key,
                    PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                    SalesOrderId = salesOrderExit.Id,
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
                    InventorySource = InventorySourceEnum.SalesOrder,
                    ProductId = cs.Key.ProductId,
                    Stock = cs.Sum(d => d.Stock),
                    WarehouseId = cs.Key.WarehouseId
                }).ToList();

            foreach (var inventory in warehouseInventoriesToAdd)
            {
                await _inventoryRepository.AddWarehouseInventory(inventory);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Sales Order.");
                return ServiceResponse<SalesOrderDto>.Return500();
            }
            var dto = _mapper.Map<SalesOrderDto>(salesOrderExit);
            return ServiceResponse<SalesOrderDto>.ReturnResultWith201(dto);
        }
    }

}
