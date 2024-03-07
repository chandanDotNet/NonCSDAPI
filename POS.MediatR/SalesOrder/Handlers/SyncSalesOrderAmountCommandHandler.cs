using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
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
    public class SyncSalesOrderAmountCommandHandler : IRequestHandler<SyncSalesOrderAmountCommand, ServiceResponse<SalesOrderDto>>
    {


        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSalesOrderDateTimeCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public SyncSalesOrderAmountCommandHandler(
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateSalesOrderDateTimeCommandHandler> logger,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }


        public async Task<ServiceResponse<SalesOrderDto>> Handle(SyncSalesOrderAmountCommand request, CancellationToken cancellationToken)
        {

            foreach (var SalesOrder in request.SalesOrder)
            {
                var salesOrderExit = _salesOrderRepository.FindBy(c => c.Id == SalesOrder.Id).FirstOrDefault();
                if (salesOrderExit != null)
                {
                    var salesOrderItemsExist = await _salesOrderItemRepository.FindBy(c => c.SalesOrderId == SalesOrder.Id).ToListAsync();
                    salesOrderExit.SalesOrderItems = salesOrderItemsExist;
                    salesOrderExit.SalesOrderItems.ForEach(c =>
                    {
                        var product = _productRepository.All.Where(x => x.Id == c.ProductId).FirstOrDefault();                       
                        c.CreatedDate = DateTime.UtcNow;
                        //c.TotalSalesPrice= decimal.Round((decimal)c.UnitPrice*c.Quantity);
                        
                        decimal value = (decimal)(product.SalesPrice) * c.Quantity;
                        int roundedValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
                        c.TotalSalesPrice = (decimal)roundedValue;
                        if (product != null)
                        {
                            //c.TotalPurPrice = Math.Round((decimal)product.PurchasePrice) * c.Quantity;
                            c.TotalPurPrice = Convert.ToDecimal((Math.Round((product.PurchasePrice.Value * c.Quantity), MidpointRounding.AwayFromZero).ToString("0.00")));
                            c.PurchasePrice = product.PurchasePrice;
                            c.UnitPrice = (decimal)product.SalesPrice;
                        }

                    });
                    salesOrderExit.TotalAmount = decimal.Round((decimal)salesOrderExit.SalesOrderItems.Sum(item => item.TotalSalesPrice));                   
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
