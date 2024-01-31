using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
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
    public class GetAllProductTypeQueryHandler : IRequestHandler<GetAllProductTypeQuery, List<ProductTypeDto>>
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMapper _mapper;

        public GetAllProductTypeQueryHandler(IProductTypeRepository productTypeRepository, IMapper mapper
            )
        {
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductTypeDto>> Handle(GetAllProductTypeQuery request, CancellationToken cancellationToken)
        {

            var productTypes = await _productTypeRepository.All
               .Select(c => new ProductTypeDto
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .Where(c => request.IsDropDown)
               .OrderBy(c => c.Name)
               .ToListAsync();

            return productTypes;
        }
    }
}