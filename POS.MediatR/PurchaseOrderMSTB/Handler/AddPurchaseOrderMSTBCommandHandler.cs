using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class AddPurchaseOrderMSTBCommandHandler : IRequestHandler<AddPurchaseOrderMSTBCommand, ServiceResponse<MSTBPurchaseOrderDto>>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPurchaseOrderMSTBCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public AddPurchaseOrderMSTBCommandHandler(
            IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddPurchaseOrderMSTBCommandHandler> logger,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse<MSTBPurchaseOrderDto>> Handle(AddPurchaseOrderMSTBCommand request, CancellationToken cancellationToken)
        {
            if (request.MSTBPurchaseOrderItems.Count > 0)
            {
                request.MSTBPurchaseOrderItems = request.MSTBPurchaseOrderItems.DistinctBy(x => x.ProductId).ToList();
                request.MSTBPurchaseOrderItems = request.MSTBPurchaseOrderItems.Where(x => x.Quantity > 0).ToList();
            }

            var existingPONumber = _mstbPurchaseOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber);
            if (existingPONumber)
            {
                return ServiceResponse<MSTBPurchaseOrderDto>.Return409("MSTB Purchase Order Number is already Exists.");
            }

            var purchaseOrder = _mapper.Map<Data.MSTBPurchaseOrder>(request);
            purchaseOrder.PaymentStatus = PaymentStatus.Pending;
            purchaseOrder.MSTBPurchaseOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.MSTBPurchaseOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
                item.CreatedDate = DateTime.Now;

                //var productDetails = _productRepository.FindBy(p => p.Id == item.ProductId).FirstOrDefault();
                //{
                //    if (productDetails != null)
                //    {
                //        productDetails.SalesPrice = (decimal)Math.Round((decimal)item.SalesPrice, MidpointRounding.AwayFromZero);
                //        productDetails.Mrp = item.Mrp;
                //        productDetails.Margin = item.Margin;
                //        productDetails.PurchasePrice = item.UnitPrice;
                //        if (request.SupplierId != new Guid("31354991-4C89-4BBF-BA52-08DC247B544B"))
                //        {
                //            productDetails.SupplierId = request.SupplierId;
                //        }

                //        _productRepository.Update(productDetails);
                //    }
                //}
                //item.CreatedDate = DateTime.UtcNow;
            });
            _mstbPurchaseOrderRepository.Add(purchaseOrder);

            //try
            //{
            //    var error = await _uow.SaveAsync();
            //}
            //catch (Exception ex)
            //{

            //    var msg = ex.Message;
            //}

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order.");
                return ServiceResponse<MSTBPurchaseOrderDto>.Return500();
            }

            var dto = _mapper.Map<MSTBPurchaseOrderDto>(purchaseOrder);
            return ServiceResponse<MSTBPurchaseOrderDto>.ReturnResultWith201(dto);
        }
    }
}
