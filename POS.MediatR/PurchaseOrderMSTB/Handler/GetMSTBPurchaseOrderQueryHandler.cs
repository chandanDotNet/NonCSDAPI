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

namespace POS.MediatR.PurchaseOrderMSTB.Handler
{
    public class GetMSTBPurchaseOrderQueryHandler : IRequestHandler<GetMSTBPurchaseOrderQuery, ServiceResponse<MSTBPurchaseOrderDto>>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;
        private readonly IMapper _mapper;

        public GetMSTBPurchaseOrderQueryHandler(IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository,
            IMapper mapper)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<MSTBPurchaseOrderDto>> Handle(GetMSTBPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _mstbPurchaseOrderRepository.All
                 .Where(c => c.Id == request.Id)
                //.Include(c => c.MSTBPurchaseOrderPayments)
                .Include(c => c.Supplier)
                //.ThenInclude(c => c.BillingAddress)
                .Include(c => c.MSTBPurchaseOrderItems)
                    //    .ThenInclude(c => c.MSTBPurchaseOrderItemTaxes)
                    //    .ThenInclude(cs => cs.Tax)
                    //.Include(c => c.MSTBPurchaseOrderItems)
                    .ThenInclude(c => c.Product)
                 .Include(c => c.MSTBPurchaseOrderItems)
                    .ThenInclude(c => c.UnitConversation)
                .FirstOrDefaultAsync();
            var dto = _mapper.Map<MSTBPurchaseOrderDto>(entity);
            return ServiceResponse<MSTBPurchaseOrderDto>.ReturnResultWith200(dto);
        }
    }
}