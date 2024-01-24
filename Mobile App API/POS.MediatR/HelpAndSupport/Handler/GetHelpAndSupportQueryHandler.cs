using AutoMapper;
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

namespace POS.MediatR.HelpAndSupport.Handler
{
    public class GetHelpAndSupportQueryHandler : IRequestHandler<GetHelpAndSupportQuery, List<HelpAndSupportDto>>
    {
        private readonly IHelpAndSupportRepository _helpAndSupportRepository;
        private readonly IMapper _mapper;

        public GetHelpAndSupportQueryHandler(IHelpAndSupportRepository helpAndSupportRepository, IMapper mapper)
        {
            _helpAndSupportRepository = helpAndSupportRepository;
            _mapper = mapper;
        }
        public async Task<List<HelpAndSupportDto>> Handle(GetHelpAndSupportQuery request, CancellationToken cancellationToken)
        {
            var entities = await _helpAndSupportRepository.All
               .Select(c => new HelpAndSupportDto
               {
                   Id = c.Id,
                   Heading = c.Heading,
                   Description = c.Description
               }).ToListAsync();

            return entities;

            //var categories = await _mainCategoryRepository.All                
            //    .OrderBy(c => c.Name)
            //    .ProjectTo<ProductMainCategoryDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            //return categories;
        }
    }
}
