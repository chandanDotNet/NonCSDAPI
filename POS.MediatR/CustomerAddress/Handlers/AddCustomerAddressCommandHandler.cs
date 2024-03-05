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
using POS.MediatR.CustomerAddress.Commands;

namespace POS.MediatR.CustomerAddress.Handlers
{
    public class AddCustomerAddressCommandHandler : 
        IRequestHandler<AddCustomerAddressCommand, ServiceResponse<CustomerAddressDto>>
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCustomerAddressCommandHandler> _logger;
        public AddCustomerAddressCommandHandler(
           ICustomerAddressRepository customerAddressRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCustomerAddressCommandHandler> logger
            )
        {
            _customerAddressRepository = customerAddressRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<CustomerAddressDto>> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {

            //var existingEntity = await _customerAddressRepository
            //    .All
            //    .FirstOrDefaultAsync(c => c.HouseNo == request.HouseNo
            //    && c.StreetDetails == request.StreetDetails
            //    && c.LandMark == request.LandMark);

            var existingEntity = await _customerAddressRepository
               .All
               .FirstOrDefaultAsync(c => c.HouseNo == request.HouseNo
                && c.CustomerId == request.CustomerId);

            if (existingEntity != null)
            {
                _logger.LogError("Customer Address Already Exist");
                return ServiceResponse<CustomerAddressDto>.Return409("Customer Address Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.CustomerAddress>(request);
            _customerAddressRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving ICustomer Address.");
                return ServiceResponse<CustomerAddressDto>.Return500();
            }
            return ServiceResponse<CustomerAddressDto>.ReturnResultWith200(_mapper.Map<CustomerAddressDto>(entity));
        }
    }
}
