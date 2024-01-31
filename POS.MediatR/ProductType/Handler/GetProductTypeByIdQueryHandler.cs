using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Packaging.Command;
using POS.MediatR.ProductType.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.ProductType.Handler
{
    public class GetProductTypeByIdQueryHandler : IRequestHandler<GetProductTypeByIdQuery, ServiceResponse<ProductTypeDto>>
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMapper _mapper;

        public GetProductTypeByIdQueryHandler(
            IProductTypeRepository productTypeRepository,
            IMapper mapper)
        {
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<ProductTypeDto>> Handle(GetProductTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var packaging = await _productTypeRepository.FindAsync(request.Id);
            if (packaging == null)
            {
                return ServiceResponse<ProductTypeDto>.Return404();
            }

            return ServiceResponse<ProductTypeDto>.ReturnResultWith200(_mapper.Map<ProductTypeDto>(packaging));
        }
    }
}