using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.MediatR.MSTBSetting.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.MSTBSetting.Handler
{
    internal class DeleteMstbSettingCommandHandler : IRequestHandler<DeleteMstbSettingCommand, ServiceResponse<bool>>
    {
        private readonly IMstbSettingRepository _mstbSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteMstbSettingCommandHandler> _logger;

        public DeleteMstbSettingCommandHandler(
            IMstbSettingRepository mstbSettingRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteMstbSettingCommandHandler> logger)
        {
            _mstbSettingRepository = mstbSettingRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteMstbSettingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _mstbSettingRepository.FindAsync(request.Id);

            if (entityExist.IsDefault)
            {
                _logger.LogError("Default setting can not be Deleted.");
                return ServiceResponse<bool>.Return409("Default setting can not be Deleted.");
            }

            if (entityExist == null)
            {
                _logger.LogError("Mstb Setting not found");
                return ServiceResponse<bool>.Return404();
            }

            _mstbSettingRepository.Delete(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting the setting.", request.Id);
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}