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
using System.IO;

namespace POS.MediatR.Category.Handlers
{
    public class AddProductCategoryCommandHandler
       : IRequestHandler<AddProductCategoryCommand, ServiceResponse<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddProductCategoryCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddProductCategoryCommandHandler(
           IProductCategoryRepository productCategoryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddProductCategoryCommandHandler> logger,
            PathHelper pathHelper,
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
        public async Task<ServiceResponse<ProductCategoryDto>> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _productCategoryRepository
                .All
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.ParentId == request.ParentId);

            if (existingEntity != null)
            {
                _logger.LogError("Product Category Already Exist");
                return ServiceResponse<ProductCategoryDto>.Return409("Product Category Already Exist.");
            }
            var entity = _mapper.Map<ProductCategory>(request);

            if (!string.IsNullOrWhiteSpace(request.ProductCategoryUrl))
            {
                entity.ProductCategoryUrl = Guid.NewGuid().ToString() + ".png";
            }
            entity.Id = Guid.NewGuid();
            _productCategoryRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving IProduct Category.");
                return ServiceResponse<ProductCategoryDto>.Return500();
            }
            if (!string.IsNullOrWhiteSpace(request.ProductCategoryUrl))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.ProductCategoryImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ProductCategoryUrl), request.ProductCategoryUrl);
            }
            return ServiceResponse<ProductCategoryDto>.ReturnResultWith200(_mapper.Map<ProductCategoryDto>(entity));
        }
    }
}
