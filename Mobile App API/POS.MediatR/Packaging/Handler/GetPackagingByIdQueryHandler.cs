using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Packaging.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Packaging.Handler
{
    public class GetPackagingByIdQueryHandler : IRequestHandler<GetPackagingByIdQuery, ServiceResponse<PackagingDto>>
    {
        private readonly IPackagingRepository _packagingRepository;
        private readonly IMapper _mapper;

        public GetPackagingByIdQueryHandler(
            IPackagingRepository packagingRepository,
            IMapper mapper)
        {
            _packagingRepository = packagingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<PackagingDto>> Handle(GetPackagingByIdQuery request, CancellationToken cancellationToken)
        {
            var packaging = await _packagingRepository.FindAsync(request.Id);
            if (packaging == null)
            {
                return ServiceResponse<PackagingDto>.Return404();
            }

            return ServiceResponse<PackagingDto>.ReturnResultWith200(_mapper.Map<PackagingDto>(packaging));
        }
    }
}