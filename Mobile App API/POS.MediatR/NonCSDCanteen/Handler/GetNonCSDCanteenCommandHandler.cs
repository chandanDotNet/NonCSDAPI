using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
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
    public class GetNonCSDCanteenCommandHandler : IRequestHandler<GetNonCSDCanteenCommand, ServiceResponse<NonCSDCanteenDto>>
    {
        private readonly INonCSDCanteenRepository _nonCSDCanteenRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetNonCSDCanteenCommandHandler> _logger;
        public GetNonCSDCanteenCommandHandler(
           INonCSDCanteenRepository nonCSDCanteenRepository,
            IMapper mapper,
            ILogger<GetNonCSDCanteenCommandHandler> logger
            )
        {
            _nonCSDCanteenRepository = nonCSDCanteenRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<NonCSDCanteenDto>> Handle(GetNonCSDCanteenCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _nonCSDCanteenRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<NonCSDCanteenDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Counter is not exists");
                return ServiceResponse<NonCSDCanteenDto>.Return404();
            }
            return ServiceResponse<NonCSDCanteenDto>.ReturnResultWith200(entityDto);
        }
    }
}