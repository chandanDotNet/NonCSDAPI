using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Category.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.IO;

namespace POS.MediatR.Category.Handlers
{
    public class UpdateProductCategoryCommandHandler
     : IRequestHandler<UpdateProductCategoryCommand, ServiceResponse<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCategoryCommandHandler> _logger;
        private readonly POS.Helper.PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpdateProductCategoryCommandHandler(
           IProductCategoryRepository productCategoryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateProductCategoryCommandHandler> logger,
            POS.Helper.PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<ServiceResponse<ProductCategoryDto>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _productCategoryRepository
                .All
                .FirstOrDefaultAsync(c => c.Name == request.Name
                && c.ParentId == request.ParentId
                && c.Id != request.Id);
            if (existingEntity != null)
            {
                _logger.LogError("Product Category Already Exist");
                return ServiceResponse<ProductCategoryDto>.Return409("Product Category Already Exist.");
            }

            existingEntity = await _productCategoryRepository.FindAsync(request.Id);

            //var entity = _mapper.Map(request, existingEntity);

            existingEntity.Name = request.Name;
            existingEntity.Description = request.Description;
            existingEntity.ParentId = request.ParentId;
            existingEntity.ProductMainCategoryId = request.ProductMainCategoryId;

            var oldImageUrl = existingEntity.ProductCategoryUrl;
            if (request.IsImageChanged)
            {
                if (!string.IsNullOrEmpty(request.ProductCategoryUrl))
                {
                    existingEntity.ProductCategoryUrl = $"{Guid.NewGuid()}.png";
                }
                else
                {
                    existingEntity.ProductCategoryUrl = "";
                }
            }
            _productCategoryRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Product Category.");
                return ServiceResponse<ProductCategoryDto>.Return500();
            }

            if (request.IsImageChanged)
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                // delete old file
                if (!string.IsNullOrWhiteSpace(oldImageUrl)
                    && File.Exists(Path.Combine(contentRootPath, _pathHelper.ProductCategoryImagePath, oldImageUrl)))
                {
                    FileData.DeleteFile(Path.Combine(contentRootPath, _pathHelper.ProductCategoryImagePath, oldImageUrl));
                }

                // save new file
                if (!string.IsNullOrWhiteSpace(request.ProductCategoryUrl))
                {
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.ProductCategoryImagePath);
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    await FileData.SaveFile(Path.Combine(pathToSave, existingEntity.ProductCategoryUrl), request.ProductCategoryUrl);
                }
            }
            return ServiceResponse<ProductCategoryDto>.ReturnResultWith200(_mapper.Map<ProductCategoryDto>(existingEntity));
        }
    }
}
