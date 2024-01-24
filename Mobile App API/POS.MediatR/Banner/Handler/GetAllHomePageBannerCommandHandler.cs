using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Banner.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Handler
{
    public class GetAllHomePageBannerCommandHandler : IRequestHandler<GetAllHomePageBannerCommand, List<HomePageBannerDto>>
    {
        private readonly IHomePageBannerRepository _homePageBannerRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllHomePageBannerCommandHandler(
           IHomePageBannerRepository homePageBannerRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _homePageBannerRepository = homePageBannerRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<HomePageBannerDto>> Handle(GetAllHomePageBannerCommand request, CancellationToken cancellationToken)
        {
            var entities = await _homePageBannerRepository.All
                .Select(c => new HomePageBannerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.HomePageBannerImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}