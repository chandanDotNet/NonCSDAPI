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

namespace POS.MediatR.AppVersion.Handler
{
    public class GetAppVersionQueryHandler : IRequestHandler<GetAppVersionQuery, List<AppVersionDto>>
    {
        private readonly IAppVersionRepository _appVersionRepository;
        private readonly IMapper _mapper;

        public GetAppVersionQueryHandler(IAppVersionRepository appVersionRepository, IMapper mapper)
        {
            _appVersionRepository = appVersionRepository;
            _mapper = mapper;
        }
        public async Task<List<AppVersionDto>> Handle(GetAppVersionQuery request, CancellationToken cancellationToken)
        {
            var entities = await _appVersionRepository.All
               .Select(c => new AppVersionDto
               {
                   Id = c.Id,
                   AppType = c.AppType,
                   Version = c.Version,
                   StoreOpenClose= c.StoreOpenClose
               }).ToListAsync();

            return entities;
        }
    }
}