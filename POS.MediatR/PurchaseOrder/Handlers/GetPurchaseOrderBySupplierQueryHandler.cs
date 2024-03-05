using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetPurchaseOrderBySupplierQueryHandler : IRequestHandler<GetPurchaseOrderBySupplierQuery, ServiceResponse<PurchaseOrderDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IMapper _mapper;

        public GetPurchaseOrderBySupplierQueryHandler(IPurchaseOrderRepository purchaseOrderRepository,
            IMapper mapper)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PurchaseOrderDto>> Handle(GetPurchaseOrderBySupplierQuery request, CancellationToken cancellationToken)
        {
            var entity = await _purchaseOrderRepository.All
                 .Where(c => c.SupplierId == request.SupplierId && c.Year == request.Year && c.Month == request.Month && c.IsAppMSTB == true)
                .Include(c => c.PurchaseOrderPayments)
                .Include(c => c.Supplier)
                    .ThenInclude(c => c.BillingAddress)
                .Include(c => c.PurchaseOrderItems)
                    .ThenInclude(c => c.PurchaseOrderItemTaxes)
                    .ThenInclude(cs => cs.Tax)
                .Include(c => c.PurchaseOrderItems)
                    .ThenInclude(c => c.Product)
                 .Include(c => c.PurchaseOrderItems)
                    .ThenInclude(c => c.UnitConversation)
                .FirstOrDefaultAsync();
            var dto = _mapper.Map<PurchaseOrderDto>(entity);
            return ServiceResponse<PurchaseOrderDto>.ReturnResultWith200(dto);
        }
    }
}