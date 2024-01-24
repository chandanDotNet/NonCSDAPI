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
    public class GetAllCategoryBannerCommandHandler : IRequestHandler<GetAllCategoryBannerCommand, List<CategoryBannerDto>>
    {
        private readonly ICategoryBannerRepository _categoryBannerRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllCategoryBannerCommandHandler(
           ICategoryBannerRepository categoryBannerRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _categoryBannerRepository = categoryBannerRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<CategoryBannerDto>> Handle(GetAllCategoryBannerCommand request, CancellationToken cancellationToken)
        {
            var entities = await _categoryBannerRepository.All
                .Select(c => new CategoryBannerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.CategoryBannerImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}