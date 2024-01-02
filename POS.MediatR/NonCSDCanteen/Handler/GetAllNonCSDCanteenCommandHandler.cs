using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.NonCSDCanteen.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.NonCSDCanteen.Handler
{
    public class GetAllNonCSDCanteenCommandHandler : IRequestHandler<GetAllNonCSDCanteenCommand, List<NonCSDCanteenDto>>
    {
        private readonly INonCSDCanteenRepository _nonCSDCanteenRepository;
        private readonly IMapper _mapper;

        public GetAllNonCSDCanteenCommandHandler(
           INonCSDCanteenRepository nonCSDCanteenRepository,
            IMapper mapper)
        {
            _nonCSDCanteenRepository = nonCSDCanteenRepository;
            _mapper = mapper;
        }

        public async Task<List<NonCSDCanteenDto>> Handle(GetAllNonCSDCanteenCommand request, CancellationToken cancellationToken)
        {
            var entities = await _nonCSDCanteenRepository.All
                .Select(c => new NonCSDCanteenDto
                {
                    Id = c.Id,
                    CanteenLocationName = c.CanteenLocationName,
                }).ToListAsync();
            return entities;
        }
    }
}
