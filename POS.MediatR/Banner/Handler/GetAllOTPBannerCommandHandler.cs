using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.MediatR.Banner.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Banner.Handler
{
    public class GetAllOTPBannerCommandHandler : IRequestHandler<GetAllOTPBannerCommand, List<OTPBannerDto>>
    {
        private readonly IOTPBannerRepository _oTpBannerRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllOTPBannerCommandHandler(
           IOTPBannerRepository oTpbannerRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _oTpBannerRepository = oTpbannerRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<OTPBannerDto>> Handle(GetAllOTPBannerCommand request, CancellationToken cancellationToken)
        {
            var entities = await _oTpBannerRepository.All
                .Select(c => new OTPBannerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.OTPBannerImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}