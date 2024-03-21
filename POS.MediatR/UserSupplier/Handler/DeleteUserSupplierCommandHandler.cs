using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
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
    public class DeleteUserSupplierCommandHandler : IRequestHandler<DeleteUserSupplierCommand, ServiceResponse<bool>>
    {

        private readonly IUserSupplierRepository _userSupplierRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteUserSupplierCommandHandler> _logger;
        public DeleteUserSupplierCommandHandler(
           IUserSupplierRepository userSupplierRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteUserSupplierCommandHandler> logger
            )
        {
            _userSupplierRepository = userSupplierRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteUserSupplierCommand request, CancellationToken cancellationToken)
        {
            var existingUserSuppliers = await _userSupplierRepository.All.Where(x => x.UserId == request.UserId).ToListAsync();
            if (existingUserSuppliers == null)
            {
                _logger.LogError("Data does not exists.");
                return ServiceResponse<bool>.Return404("Data does not exists.");
            }
            _userSupplierRepository.RemoveRange(existingUserSuppliers);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While deleting user supplier.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }

    }
}