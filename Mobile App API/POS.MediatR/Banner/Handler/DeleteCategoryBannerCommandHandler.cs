using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Banner.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Handler
{
    public class DeleteCategoryBannerCommandHandler : IRequestHandler<DeleteCategoryBannerCommand, ServiceResponse<bool>>
    {
        private readonly ICategoryBannerRepository _categoryBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteCategoryBannerCommandHandler> _logger;

        public DeleteCategoryBannerCommandHandler(
            ICategoryBannerRepository categoryBannerRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCategoryBannerCommandHandler> logger
            )
        {
            _categoryBannerRepository = categoryBannerRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCategoryBannerCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _categoryBannerRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Category banner Does not exists");
                return ServiceResponse<bool>.Return404("Category banner Does not exists");
            }
            _categoryBannerRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Banner.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}