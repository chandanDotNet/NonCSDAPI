using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
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
    public class AddHomePageBannerCommandHandler : IRequestHandler<AddHomePageBannerCommand, ServiceResponse<HomePageBannerDto>>
    {
        private readonly IHomePageBannerRepository _homePageBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddHomePageBannerCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddHomePageBannerCommandHandler(
            IHomePageBannerRepository homePageBannerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddHomePageBannerCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _homePageBannerRepository = homePageBannerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<HomePageBannerDto>> Handle(AddHomePageBannerCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _homePageBannerRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Home Page Banner Already Exist");
                return ServiceResponse<HomePageBannerDto>.Return409("Home Page Banner Already Exist.");
            }
            var entity = _mapper.Map<Data.HomePageBanner>(request);

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }

            _homePageBannerRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<HomePageBannerDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.HomePageBannerImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }
            var entityToReturn = _mapper.Map<HomePageBannerDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entityToReturn.ImageUrl = Path.Combine(_pathHelper.HomePageBannerImagePath, entityToReturn.ImageUrl);
            }
            return ServiceResponse<HomePageBannerDto>.ReturnResultWith200(entityToReturn);
        }
    }
}