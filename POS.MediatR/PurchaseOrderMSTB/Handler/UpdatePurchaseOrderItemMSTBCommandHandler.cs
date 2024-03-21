using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.PurchaseOrderMSTB.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class UpdatePurchaseOrderItemMSTBCommandHandler : IRequestHandler<UpdatePurchaseOrderItemMSTBCommand, ServiceResponse<MSTBPurchaseOrderItemDto>>
    {
        private readonly IMSTBPurchaseOrderItemRepository _mstbPurchaseOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdatePurchaseOrderItemMSTBCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UpdatePurchaseOrderItemMSTBCommandHandler(
           IMSTBPurchaseOrderItemRepository mstbPurchaseOrderItemRepository,
           IUnitOfWork<POSDbContext> uow,
           ILogger<UpdatePurchaseOrderItemMSTBCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment,
        IMapper mapper
           )
        {
            _mstbPurchaseOrderItemRepository = mstbPurchaseOrderItemRepository;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<MSTBPurchaseOrderItemDto>> Handle(UpdatePurchaseOrderItemMSTBCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _mstbPurchaseOrderItemRepository.FindBy(c => c.Id == request.Id)
             .FirstOrDefaultAsync();
            //if (entityExist != null)
            //{
            //    _logger.LogError("Customer Address Already Exist.");
            //    return ServiceResponse<CustomerAddressDto>.Return409("Customer Address Already Exist.");
            //}
            //entityExist = await _customerAddressRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();

            if (request.Approved == null || request.Approved == true)
            {
                entityExist.IsCheck = request.IsCheck;
                entityExist.Margin = request.Margin;
                entityExist.SalesPrice = request.SalesPrice;
                entityExist.Difference = request.Difference;
                entityExist.Surplus = request.Surplus;
                entityExist.NewQuantity = request.Quantity;

                if (entityExist.NewMRP != request.MRP)
                {
                    entityExist.NewMRP = request.MRP;
                    entityExist.IsMRPChanged = true;
                }
            }
            
            if (request.UserType == "Agent")
            {
                entityExist.Approved = string.Empty;
            }

            if (request.UserType == "Reviewer")
            {
                if (request.Approved == false)
                {
                    entityExist.Approved = "Rejected";
                    //entityExist.IsCheck = false;

                }
                if (request.Approved == true)
                {
                    entityExist.Approved = "Approved";
                    entityExist.Mrp = request.MRP;
                }
            }            

            _mstbPurchaseOrderItemRepository.Update(entityExist);

            //remove other as Primary Address
            //if (entityExist.IsPrimary)
            //{
            //    var defaultPrimaryAddressSettings = await _customerAddressRepository.All.Where(c => c.CustomerId == request.CustomerId && c.Id != request.Id).ToListAsync();
            //    defaultPrimaryAddressSettings.ForEach(c => c.IsPrimary = false);
            //    _customerAddressRepository.UpdateRange(defaultPrimaryAddressSettings);
            //}

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<MSTBPurchaseOrderItemDto>.Return500();
            }

            var result = _mapper.Map<MSTBPurchaseOrderItemDto>(entityExist);

            return ServiceResponse<MSTBPurchaseOrderItemDto>.ReturnResultWith200(result);
        }
    }
}