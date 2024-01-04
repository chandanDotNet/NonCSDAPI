using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
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

namespace POS.MediatR.Inventory.Handler
{
    internal class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, ServiceResponse<bool>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IProductRepository _productRepository;

        public AddInventoryCommandHandler(IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _uow = uow;
            _productRepository = productRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.ProductCode))
            {
                var findProduct = await _productRepository.FindBy(c => c.Code == request.ProductCode)
                    .FirstOrDefaultAsync();

                if (findProduct != null)
                {
                    request.ProductId = findProduct.Id;
                    request.UnitId = findProduct.UnitId;

                    var inventory = _mapper.Map<InventoryDto>(request);
                    inventory.InventorySource = InventorySourceEnum.Direct;
                    inventory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(inventory);
                    await _inventoryRepository.AddInventory(inventory);
                    await _inventoryRepository.AddWarehouseInventory(inventory);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        return ServiceResponse<bool>.Return500();
                    }

                    //For change product Mrp and SalesPrice  
                    if (request.ProductId.HasValue)
                    {
                        var productDetails = await _productRepository.FindBy(c => c.Id == request.ProductId)
                            .FirstOrDefaultAsync();

                        if (request.PricePerUnit > 0)
                        {
                            productDetails.SalesPrice = request.PricePerUnit;
                        }
                        if (request.Mrp > 0)
                        {
                            productDetails.Mrp = request.Mrp;
                        }
                        if (request.Margin > 0)
                        {
                            productDetails.Margin = request.Margin;
                        }
                        if (request.PurchasePrice > 0)
                        {
                            productDetails.PurchasePrice = request.PurchasePrice;
                        }

                        _productRepository.Update(productDetails);
                        if (await _uow.SaveAsync() <= 0)
                        {
                            return ServiceResponse<bool>.Return500();
                        }
                    }

                }
            }
            

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
