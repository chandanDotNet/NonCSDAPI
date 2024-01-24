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
    public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, ServiceResponse<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteBannerCommandHandler> _logger;

        public DeleteBannerCommandHandler(
            IBannerRepository bannerRepository,
            IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteBannerCommandHandler> logger
            )
        {
            _bannerRepository = bannerRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _bannerRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Banner Does not exists");
                return ServiceResponse<bool>.Return404("Banner Does not exists");
            }
            _bannerRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Banner.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}