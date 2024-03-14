using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.UserSupplier.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.UserSupplier.Handler
{
    public class AddUserSupplierCommandHandler : IRequestHandler<AddUserSupplierCommand, ServiceResponse<UserSupplierDto>>
    {
        private readonly IUserSupplierRepository _userSupplierRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddUserSupplierCommandHandler> _logger;

        public AddUserSupplierCommandHandler(
           IUserSupplierRepository userSupplierRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddUserSupplierCommandHandler> logger
            )
        {
            _userSupplierRepository = userSupplierRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<UserSupplierDto>> Handle(AddUserSupplierCommand request, CancellationToken cancellationToken)
        {
            //var existingEntity = await _userSupplierRepository.FindBy(c => c.UserId == request.).FirstOrDefaultAsync();
            //if (existingEntity != null)
            //{
            //    _logger.LogError("Banner Already Exist");
            //    return ServiceResponse<BannerDto>.Return409("Banner Already Exist.");
            //}
            var entity = _mapper.Map<List<Data.UserSupplier>>(request.UserSuppliers);

            _userSupplierRepository.AddRange(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<UserSupplierDto>.Return500();
            }

            
            var entityToReturn = _mapper.Map<List<UserSupplierDto>>(entity);
           
            return ServiceResponse<UserSupplierDto>.ReturnResultWith200(entityToReturn.FirstOrDefault());
        }
    }
}