using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Banner.Command;
using POS.MediatR.PaymentCard.Commands;
using POS.MediatR.PaymentCard.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Handler
{
    public class DeleteLoginPageBannerCommandHandler : IRequestHandler<DeleteLoginPageBannerCommand, ServiceResponse<bool>>
    {
        private readonly ILoginPageBannerRepository _loginPageBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteLoginPageBannerCommandHandler> _logger;

        public DeleteLoginPageBannerCommandHandler(
            ILoginPageBannerRepository loginPageBannerRepository,
            IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteLoginPageBannerCommandHandler> logger
            )
        {
            _loginPageBannerRepository = loginPageBannerRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteLoginPageBannerCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _loginPageBannerRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Login Page Banner Does not exists");
                return ServiceResponse<bool>.Return404("Login Page Banner Does not exists");
            }
            _loginPageBannerRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Login Page Banner.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}