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
    public class DeleteHomePageBannerCommandHandler : IRequestHandler<DeleteHomePageBannerCommand, ServiceResponse<bool>>
    {
        private readonly IHomePageBannerRepository _homePageBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteHomePageBannerCommandHandler> _logger;

        public DeleteHomePageBannerCommandHandler(
            IHomePageBannerRepository homePageBannerRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteHomePageBannerCommandHandler> logger
            )
        {
            _homePageBannerRepository = homePageBannerRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteHomePageBannerCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _homePageBannerRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Home Page Banner Does not exists");
                return ServiceResponse<bool>.Return404("Home Page banner Does not exists");
            }
            _homePageBannerRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Banner.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}