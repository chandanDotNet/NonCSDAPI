using AutoMapper;
using ExcelDataReader.Log;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddSalesOrderCommandHandler : IRequestHandler<AddSalesOrderCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddSalesOrderCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        public AddSalesOrderCommandHandler(
            ISalesOrderRepository salesOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddSalesOrderCommandHandler> logger,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse<SalesOrderDto>> Handle(AddSalesOrderCommand request, CancellationToken cancellationToken)
        {

            var existingSONumber = _salesOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber);
            if (existingSONumber)
            {
                return ServiceResponse<SalesOrderDto>.Return409("Sales Order Number is already Exists.");
            }

            //request.SalesOrderItems.ForEach(item1 =>
            //{
            //    var product1 = _productRepository.All.Where(c => c.Id == item1.ProductId);
            //    item1.Product = _mapper.Map<ProductDto>(product1);// (ProductDto)_productRepository.All.Where(c => c.Id == item1.ProductId);                

            //});

            var salesOrder = _mapper.Map<Data.SalesOrder>(request);
            salesOrder.PaymentStatus = PaymentStatus.Pending;
            salesOrder.OrderDeliveryStatus = "Order Placed";
            if (salesOrder.IsAdvanceOrderRequest == true)
            {
                salesOrder.SOCreatedDate = request.DeliveryDate;
            }
            else
            {
                if (DateTime.Now.TimeOfDay.Hours >= 17 && DateTime.Now.TimeOfDay.Minutes > 0)
                {
                    salesOrder.SOCreatedDate = DateTime.Now.AddDays(1);
                }
                else
                {
                    salesOrder.SOCreatedDate = DateTime.Now;
                }                
            }            
            //salesOrder.SalesOrderItems.ForEach(item =>
            //{
            //    var product = _productRepository.All.Where(c => c.Id == item.ProductId).FirstOrDefault();
            //    item.Product = null;
            //    item.Warehouse = null;
            //    item.SalesOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
            //    item.CreatedDate = DateTime.Now;
            //    if (product != null)
            //    {
            //        item.TotalPurPrice = Math.Round((decimal)product.PurchasePrice) * item.Quantity;

            //        //decimal aa = (decimal)(item.UnitPrice) * item.Quantity;
            //        //decimal ff=decimal.Round((decimal)aa);
            //        decimal value = (decimal)(item.UnitPrice * item.Quantity);
            //        int roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
            //        item.TotalSalesPrice = (decimal)roundedValue;

            //    }
            //    //if (item.LooseQuantity>0)
            //    //{
            //    //    item.LooseQuantity = item.LooseQuantity * 1000;
            //    //    //item.UnitId=
            //    //}
            //});

            //==================

            if (salesOrder.SalesOrderItems.Count > 0)
            {
                for (int i = 0; i < salesOrder.SalesOrderItems.Count; i++)
                {
                    decimal TotalPurPrice = 0, value=0;
                    int roundedValue = 0;
                    var product = _productRepository.All.Where(c => c.Id == salesOrder.SalesOrderItems[i].ProductId).FirstOrDefault();
                    if (product != null)
                    {
                        TotalPurPrice= Math.Round((decimal)product.PurchasePrice) * salesOrder.SalesOrderItems[i].Quantity;
                    }

                    value = (decimal)(salesOrder.SalesOrderItems[i].UnitPrice) * salesOrder.SalesOrderItems[i].Quantity;
                    roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                    salesOrder.SalesOrderItems[i].TotalSalesPrice = roundedValue;
                    salesOrder.SalesOrderItems[i].TotalPurPrice = TotalPurPrice;

                    salesOrder.SalesOrderItems[i].Product = null;
                    salesOrder.SalesOrderItems[i].Warehouse = null;
                    salesOrder.SalesOrderItems[i].SalesOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
                    salesOrder.SalesOrderItems[i].CreatedDate = DateTime.Now;
                    //entity.SalesOrderItems[i].TotalSalesPrice = decimal.Round((decimal)(entity.SalesOrderItems[i].UnitPrice * entity.SalesOrderItems[i].Quantity));
                    _logger.LogError("TotalSalesPrice -", roundedValue);
                }
            }

            //==================
            decimal TotalAmount = 0;
            TotalAmount = (decimal)salesOrder.SalesOrderItems.Sum(item => item.TotalSalesPrice);
            salesOrder.TotalAmount = (decimal)Math.Round(TotalAmount, MidpointRounding.AwayFromZero);

            _logger.LogError("TotalAmount -", TotalAmount);

            _salesOrderRepository.Add(salesOrder);


            var inventories = request.SalesOrderItems

                .Select(cs => new InventoryDto
                {
                    ProductId = cs.ProductId,
                    PricePerUnit = cs.UnitPrice,
                    SalesOrderId = salesOrder.Id,
                    Stock = cs.Quantity,
                    UnitId = cs.UnitId,
                    WarehouseId = cs.WarehouseId,
                    TaxValue = cs.TaxValue,
                    Discount = cs.Discount,
                    //Product= _mapper.Map<ProductDto>(cs.Product)
                    PurchasePrice = _productRepository.All.Where(c => c.Id == cs.ProductId).FirstOrDefault().PurchasePrice,
                    //PurchasePrice = cs.PurPrice,
                    Mrp = _productRepository.All.Where(c => c.Id == cs.ProductId).FirstOrDefault().Mrp,
                    Margin = _productRepository.All.Where(c => c.Id == cs.ProductId).FirstOrDefault().Margin
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
                    SalesOrderId = salesOrder.Id,
                    Stock = cs.Sum(d => d.Stock),
                    PurchasePrice = cs.FirstOrDefault().PurchasePrice,
                    Margin = cs.FirstOrDefault().Margin,
                    Mrp = cs.FirstOrDefault().Mrp
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

            //var aa = _uow.SaveAsync();


            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Sales Order.");
                return ServiceResponse<SalesOrderDto>.Return500();
            }
            var dto = _mapper.Map<SalesOrderDto>(salesOrder);
            return ServiceResponse<SalesOrderDto>.ReturnResultWith201(dto);
        }
    }
}
