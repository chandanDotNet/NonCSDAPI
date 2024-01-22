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

namespace POS.MediatR.Packaging.Handler
{
    public class GetAllPackagingQueryHandler : IRequestHandler<GetAllPackagingQuery, List<PackagingDto>>
    {
        private readonly IPackagingRepository _packagingRepository;
        private readonly IMapper _mapper;

        public GetAllPackagingQueryHandler(IPackagingRepository packagingRepository, IMapper mapper
            )
        {
            _packagingRepository = packagingRepository;
            _mapper = mapper;
        }
        public async Task<List<PackagingDto>> Handle(GetAllPackagingQuery request, CancellationToken cancellationToken)
        {           

            var packagings = await _packagingRepository.All
               .Select(c => new PackagingDto
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .Where(c => request.IsDropDown)
               .OrderBy(c => c.Name)
               .ToListAsync();

            return packagings;
        }
    }
}