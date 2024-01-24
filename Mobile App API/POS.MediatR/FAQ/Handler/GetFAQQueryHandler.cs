using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.FAQ.Handler
{
    public class GetFAQQueryHandler : IRequestHandler<GetFAQQuery, List<FAQDto>>
    {
        private readonly IFAQRepository _fAQRepository;
        private readonly IMapper _mapper;

        public GetFAQQueryHandler(IFAQRepository fAQRepository, IMapper mapper)
        {
            _fAQRepository = fAQRepository;
            _mapper = mapper;
        }
        public async Task<List<FAQDto>> Handle(GetFAQQuery request, CancellationToken cancellationToken)
        {
            var entities = await _fAQRepository.All
               .Select(c => new FAQDto
               {
                   Id = c.Id,
                   Topic = c.Topic,
                   Question = c.Question,
                   Answer = c.Answer,
               }).ToListAsync();

            return entities;
        }
    }
}