using System;
using POS.MediatR.CommandAndQuery;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Repository;
using System.Threading;
using MediatR;
using POS.Data.Resources;

namespace POS.MediatR.Brand.Handler
{
    public class SearchBrandQueryHandler : IRequestHandler<SearchBrandQuery, BrandList>
    {
        private readonly IBrandRepository _brandRepository;

        public SearchBrandQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<BrandList> Handle(SearchBrandQuery request, CancellationToken cancellationToken)
        {
            //var brands = await _brandRepository.All.Where(a => EF.Functions.Like(a.Name, $"{request.SearchQuery}%"))
            //    .Take(request.PageSize)
            //    .Select(c => new BrandDto
            //    {
            //        Id = c.Id,
            //        Name = c.Name
            //    }).ToListAsync();
            //return brands;
            return await _brandRepository.GetBrands(request.BrandResource);
        }
    }
}