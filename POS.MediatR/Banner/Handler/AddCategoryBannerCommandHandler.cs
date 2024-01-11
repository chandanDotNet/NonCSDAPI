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
    public class AddCategoryBannerCommandHandler : IRequestHandler<AddCategoryBannerCommand, ServiceResponse<CategoryBannerDto>>
    {
        private readonly ICategoryBannerRepository _categoryBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCategoryBannerCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddCategoryBannerCommandHandler(
            ICategoryBannerRepository categoryBannerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCategoryBannerCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _categoryBannerRepository = categoryBannerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<CategoryBannerDto>> Handle(AddCategoryBannerCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _categoryBannerRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Category Banner Already Exist");
                return ServiceResponse<CategoryBannerDto>.Return409("Category Banner Already Exist.");
            }
            var entity = _mapper.Map<Data.CategoryBanner>(request);

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }

            _categoryBannerRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<CategoryBannerDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.CategoryBannerImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }
            var entityToReturn = _mapper.Map<CategoryBannerDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entityToReturn.ImageUrl = Path.Combine(_pathHelper.CategoryBannerImagePath, entityToReturn.ImageUrl);
            }
            return ServiceResponse<CategoryBannerDto>.ReturnResultWith200(entityToReturn);
        }
    }
}