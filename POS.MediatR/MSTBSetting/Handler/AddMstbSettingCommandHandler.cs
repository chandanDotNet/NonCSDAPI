using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
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
    public class AddMstbSettingCommandHandler :
        IRequestHandler<AddMstbSettingCommand, ServiceResponse<MstbSettingDto>>
    {
        private readonly IMstbSettingRepository _mstbSettingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMstbSettingCommandHandler> _logger;
        public AddMstbSettingCommandHandler(
           IMstbSettingRepository mstbSettingRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddMstbSettingCommandHandler> logger
            )
        {
            _mstbSettingRepository = mstbSettingRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<MstbSettingDto>> Handle(AddMstbSettingCommand request, CancellationToken cancellationToken)
        {
            //var existingEntity = await _customerAddressRepository
            //    .All
            //    .FirstOrDefaultAsync(c => c.HouseNo == request.HouseNo
            //    && c.StreetDetails == request.StreetDetails
            //    && c.LandMark == request.LandMark);

            var existingEntity = await _mstbSettingRepository
               .All
               .FirstOrDefaultAsync(c => c.Month == request.Month
                && c.Year == request.Year);

            if (existingEntity != null)
            {
                _logger.LogError("This setting already Exist");
                return ServiceResponse<MstbSettingDto>.Return409("This setting already Exist.");
            }
            var entity = _mapper.Map<Data.MstbSetting>(request);
            _mstbSettingRepository.Add(entity);           
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving the settings.");
                return ServiceResponse<MstbSettingDto>.Return500();
            }
            return ServiceResponse<MstbSettingDto>.ReturnResultWith200(_mapper.Map<MstbSettingDto>(entity));
        }
    }
}