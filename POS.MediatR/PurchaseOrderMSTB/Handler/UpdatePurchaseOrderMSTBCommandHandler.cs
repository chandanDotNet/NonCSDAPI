using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.PurchaseOrderMSTB.Command;
using POS.MediatR.PurchaseOrderMSTB.Handler;
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
    public class UpdatePurchaseOrderMSTBCommandHandler : IRequestHandler<UpdatePurchaseOrderMSTBCommand, ServiceResponse<MSTBPurchaseOrderDto>>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;
        private readonly IMSTBPurchaseOrderItemRepository _mstbPurchaseOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePurchaseOrderMSTBCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public UpdatePurchaseOrderMSTBCommandHandler(
            IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository,
            IMSTBPurchaseOrderItemRepository mstbPurchaseOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdatePurchaseOrderMSTBCommandHandler> logger,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
            _mstbPurchaseOrderItemRepository = mstbPurchaseOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse<MSTBPurchaseOrderDto>> Handle(UpdatePurchaseOrderMSTBCommand request, CancellationToken cancellationToken)
        {
            if (request.MSTBPurchaseOrderItems.Count > 0)
            {
                request.MSTBPurchaseOrderItems = request.MSTBPurchaseOrderItems.DistinctBy(x => x.ProductId).ToList();
                request.MSTBPurchaseOrderItems = request.MSTBPurchaseOrderItems.Where(x => x.Quantity > 0).ToList();
            }

            var existingPONumber = _mstbPurchaseOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber && c.Id != request.Id);
            if (existingPONumber)
            {
                return ServiceResponse<MSTBPurchaseOrderDto>.Return409("Purchase Order Number is already Exists.");
            }

            var purchaseOrderExit = await _mstbPurchaseOrderRepository.FindAsync(request.Id);
            if (purchaseOrderExit.Status == Data.PurchaseOrderStatus.Return)
            {
                return ServiceResponse<MSTBPurchaseOrderDto>.Return409("Purchase Order can't edit becuase it's already approved.");
            }

            var purchaseOrderItemsExist = await _mstbPurchaseOrderItemRepository.FindBy(c => c.PurchaseOrderId == request.Id).ToListAsync();

            // remove existing warehouse inventory           

            _mstbPurchaseOrderItemRepository.RemoveRange(purchaseOrderItemsExist);

            var purchaseOrderUpdate = _mapper.Map<POS.Data.MSTBPurchaseOrder>(request);
            purchaseOrderExit.OrderNumber = purchaseOrderUpdate.OrderNumber;
            purchaseOrderExit.SupplierId = purchaseOrderUpdate.SupplierId;
            purchaseOrderExit.Note = purchaseOrderUpdate.Note;
            purchaseOrderExit.TermAndCondition = purchaseOrderUpdate.TermAndCondition;
            purchaseOrderExit.IsPurchaseOrderRequest = purchaseOrderUpdate.IsPurchaseOrderRequest;
            purchaseOrderExit.POCreatedDate = purchaseOrderUpdate.POCreatedDate;
            purchaseOrderExit.Status = purchaseOrderUpdate.Status;
            purchaseOrderExit.DeliveryDate = purchaseOrderUpdate.DeliveryDate;
            purchaseOrderExit.DeliveryStatus = purchaseOrderUpdate.DeliveryStatus;
            purchaseOrderExit.SupplierId = purchaseOrderUpdate.SupplierId;
            purchaseOrderExit.TotalAmount = purchaseOrderUpdate.TotalAmount;
            purchaseOrderExit.TotalTax = purchaseOrderUpdate.TotalTax;
            purchaseOrderExit.TotalDiscount = purchaseOrderUpdate.TotalDiscount;

            purchaseOrderExit.PurchasePaymentType = purchaseOrderUpdate.PurchasePaymentType;
            purchaseOrderExit.InvoiceNo = purchaseOrderUpdate.InvoiceNo;
            purchaseOrderExit.TotalSaleAmount = purchaseOrderUpdate.TotalSaleAmount;
            purchaseOrderExit.Year = purchaseOrderUpdate.Year;
            purchaseOrderExit.Month = purchaseOrderUpdate.Month;
            purchaseOrderExit.IsMstbGRN = purchaseOrderUpdate.IsMstbGRN;

            purchaseOrderExit.MSTBPurchaseOrderItems = purchaseOrderUpdate.MSTBPurchaseOrderItems;
            purchaseOrderExit.MSTBPurchaseOrderItems.ForEach(c =>
            {
                c.PurchaseOrderId = purchaseOrderUpdate.Id;
            });
            purchaseOrderExit.MSTBPurchaseOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.MSTBPurchaseOrderItemTaxes.ForEach(tax => { tax.Tax = null; });

                //var productDetails = _productRepository.FindBy(p => p.Id == item.ProductId).FirstOrDefault();
                //{
                //    if (productDetails != null)
                //    {
                //        productDetails.SalesPrice = (decimal)Math.Round((decimal)item.SalesPrice, MidpointRounding.AwayFromZero);
                //        productDetails.Mrp = item.Mrp;
                //        productDetails.Margin = item.Margin;
                //        productDetails.PurchasePrice = item.UnitPrice;
                //        if (request.SupplierId != new Guid("31354991-4C89-4BBF-BA52-08DC247B544B")) //Open Stock
                //        {
                //            productDetails.SupplierId = request.SupplierId;
                //        }
                //        _productRepository.Update(productDetails);
                //    }
                //}

            });
            _mstbPurchaseOrderRepository.Update(purchaseOrderExit);
          
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order.");
                return ServiceResponse<MSTBPurchaseOrderDto>.Return500();
            }

            //============

            //if (purchaseOrderExit.PurchaseOrderItems.Count > 0)
            //{
            //    var productlist = purchaseOrderExit.PurchaseOrderItems.DistinctBy(x => x.ProductId).ToList();

            //    foreach (var item in productlist)
            //    {
            //        var productDetails = await _productRepository.FindBy(p => p.Id == item.ProductId)
            //            .FirstOrDefaultAsync();
            //        productDetails.SalesPrice = (decimal)Math.Round((decimal)item.SalesPrice, MidpointRounding.AwayFromZero);
            //        productDetails.Mrp = item.Mrp;
            //        productDetails.Margin = item.Margin;
            //        productDetails.PurchasePrice = item.UnitPrice;
            //        productDetails.SupplierId = request.SupplierId;
            //        _productRepository.Update(productDetails);
            //        if (await _uow.SaveAsync() <= 0)
            //        {
            //            return ServiceResponse<PurchaseOrderDto>.Return500();
            //        }
            //    }
            //}

            var dto = _mapper.Map<MSTBPurchaseOrderDto>(purchaseOrderExit);
            return ServiceResponse<MSTBPurchaseOrderDto>.ReturnResultWith201(dto);
        }
    }
}
