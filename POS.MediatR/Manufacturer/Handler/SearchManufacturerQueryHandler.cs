using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Manufacturer.Handler
{
    public class SearchManufacturerQueryHandler : IRequestHandler<SearchManufacturerQuery, ManufacturerList>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        public SearchManufacturerQueryHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task<ManufacturerList> Handle(SearchManufacturerQuery request, CancellationToken cancellationToken)
        {
            //var manufacturers = await _manufacturerRepository.All.Where(a => EF.Functions.Like(a.ManufacturerName, $"{request.SearchQuery}%"))
            //    .Take(request.PageSize)
            //    .Select(c => new ManufacturerDto
            //    {
            //        Id = c.Id,
            //        ManufacturerName = c.ManufacturerName
            //    }).ToListAsync();
            //return manufacturers;
            return await _manufacturerRepository.GetManufacturers(request.ManufacturerResource);
        }
    }
}
