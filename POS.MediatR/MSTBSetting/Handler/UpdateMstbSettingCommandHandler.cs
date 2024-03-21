using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CustomerAddress.Commands;
using POS.MediatR.CustomerAddress.Handlers;
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
    public class UpdateMstbSettingCommandHandler : IRequestHandler<UpdateMstbSettingCommand, ServiceResponse<MstbSettingDto>>
    {
        private readonly IMstbSettingRepository _mstbSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateMstbSettingCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateMstbSettingCommandHandler(
           IMstbSettingRepository mstbSettingRepository,
           IUnitOfWork<POSDbContext> uow,
           ILogger<UpdateMstbSettingCommandHandler> logger,
        IMapper mapper
           )
        {
            _mstbSettingRepository = mstbSettingRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<MstbSettingDto>> Handle(UpdateMstbSettingCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _mstbSettingRepository.FindBy(c => c.Id == request.Id)
             .FirstOrDefaultAsync();
            //if (entityExist != null)
            //{
            //    _logger.LogError("Customer Address Already Exist.");
            //    return ServiceResponse<CustomerAddressDto>.Return409("Customer Address Already Exist.");
            //}
            //entityExist = await _customerAddressRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();

            entityExist.IsDefault = true;

            _mstbSettingRepository.Update(entityExist);

            //remove other as Primary Address
            var defaultPrimaryAddressSettings = await _mstbSettingRepository.All.Where(c => c.Id != request.Id && c.IsDefault == true).ToListAsync();
            if (defaultPrimaryAddressSettings.Count > 0 )
            {
                 defaultPrimaryAddressSettings.ForEach(c => c.IsDefault = false);
                _mstbSettingRepository.UpdateRange(defaultPrimaryAddressSettings);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<MstbSettingDto>.Return500();
            }

            var result = _mapper.Map<MstbSettingDto>(entityExist);

            return ServiceResponse<MstbSettingDto>.ReturnResultWith200(result);
        }
    }
}