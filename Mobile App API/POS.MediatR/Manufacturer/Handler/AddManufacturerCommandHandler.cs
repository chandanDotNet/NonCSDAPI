using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Manufacturer.Command;
using POS.MediatR.Manufacturer.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Manufacturer.Handler
{
    public class AddManufacturerCommandHandler : IRequestHandler<AddManufacturerCommand, ServiceResponse<ManufacturerDto>>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddManufacturerCommandHandler> _logger;

        public AddManufacturerCommandHandler(
           IManufacturerRepository manufacturerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddManufacturerCommandHandler> logger
            )
        {
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<ManufacturerDto>> Handle(AddManufacturerCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _manufacturerRepository.FindBy(c => c.ManufacturerName == request.ManufacturerName).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Manufacturer Already Exist");
                return ServiceResponse<ManufacturerDto>.Return409("Manufacturer Already Exist.");
            }
            var entity = _mapper.Map<Data.Manufacturer>(request);

            _manufacturerRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<ManufacturerDto>.Return500();
            }            
            var entityToReturn = _mapper.Map<ManufacturerDto>(entity);
            
            return ServiceResponse<ManufacturerDto>.ReturnResultWith200(entityToReturn);
        }
    }
}