using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.ProductType.Handler
{
    internal class SearchProductTypeQueryHandler : IRequestHandler<SearchProductTypeQuery, ProductTypeList>
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public SearchProductTypeQueryHandler(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }
        public async Task<ProductTypeList> Handle(SearchProductTypeQuery request, CancellationToken cancellationToken)
        {
            //var manufacturers = await _manufacturerRepository.All.Where(a => EF.Functions.Like(a.ManufacturerName, $"{request.SearchQuery}%"))
            //    .Take(request.PageSize)
            //    .Select(c => new ManufacturerDto
            //    {
            //        Id = c.Id,
            //        ManufacturerName = c.ManufacturerName
            //    }).ToListAsync();
            //return manufacturers;
            return await _productTypeRepository.GetProductTypes(request.ProductTypeResource);
        }
    }
}
