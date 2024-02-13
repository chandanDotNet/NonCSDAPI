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

namespace POS.MediatR.Year.Handler
{
    public class GetAllYearQueryHandler : IRequestHandler<GetAllYearQuery, List<YearDto>>
    {
        private readonly IYearRepository _yearRepository;
        private readonly IMapper _mapper;

        public GetAllYearQueryHandler(IYearRepository yearRepository, IMapper mapper
            )
        {
            _yearRepository = yearRepository;
            _mapper = mapper;
        }
        public async Task<List<YearDto>> Handle(GetAllYearQuery request, CancellationToken cancellationToken)
        {
            var years = await _yearRepository.All
               .Select(c => new YearDto
               {
                   Id = c.Id,
                   Name = c.Name,
                   DefaultYear= c.DefaultYear
               })               
               .OrderBy(c => c.Name)
               .ToListAsync();

            return years;
        }
    }
}