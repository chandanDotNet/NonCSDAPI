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
    internal class GetMSTBReviewerPurchaseOrderQueryHandler : IRequestHandler<GetMSTBReviewerPurchaseOrderQuery, ServiceResponse<MSTBPurchaseOrderDto>>
    {
        private readonly IMSTBPurchaseOrderRepository _mstbPurchaseOrderRepository;
        private readonly IMapper _mapper;

        public GetMSTBReviewerPurchaseOrderQueryHandler(IMSTBPurchaseOrderRepository mstbPurchaseOrderRepository,
            IMapper mapper)
        {
            _mstbPurchaseOrderRepository = mstbPurchaseOrderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<MSTBPurchaseOrderDto>> Handle(GetMSTBReviewerPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _mstbPurchaseOrderRepository.All
                 .Where(c => c.SupplierId == request.SupplierId && c.Year == request.Year && c.Month == request.Month)
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

            //entity = entity.MSTBPurchaseOrderItems
            //       .Where(a => EF.Functions.Like(a.Product.Name, $"{request.ProductName}%"));

            return ServiceResponse<MSTBPurchaseOrderDto>.ReturnResultWith200(dto);
        }
    }
}