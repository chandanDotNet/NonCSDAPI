using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.UserSupplier.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.UserSupplier.Handler
{
    public class UpdateUserSupplierCommandHandler : IRequestHandler<UpdateUserSupplierCommand, ServiceResponse<UserSupplierDto>>
    {
        private readonly IUserSupplierRepository _userSupplierRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserSupplierCommandHandler> _logger;

        public UpdateUserSupplierCommandHandler(
           IUserSupplierRepository userSupplierRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateUserSupplierCommandHandler> logger
            )
        {
            _userSupplierRepository = userSupplierRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<UserSupplierDto>> Handle(UpdateUserSupplierCommand request, CancellationToken cancellationToken)
        {           

            var existingUserSuppliers = await _userSupplierRepository.All.Where(x => x.UserId == request.UserSuppliers.FirstOrDefault().UserId).ToListAsync();
            
            if (existingUserSuppliers != null)
            {
                _userSupplierRepository.RemoveRange(existingUserSuppliers);
            }            

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