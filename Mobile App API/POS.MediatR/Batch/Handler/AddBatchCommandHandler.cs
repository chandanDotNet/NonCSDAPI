using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Batch.Command;
using POS.MediatR.Batch.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Batch.Handler
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, ServiceResponse<BatchDto>>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBatchCommandHandler> _logger;
        public AddBatchCommandHandler(
           IBatchRepository batchRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddBatchCommandHandler> logger
            )
        {
            _batchRepository = batchRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<BatchDto>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _batchRepository.FindBy(c => c.BatchName == request.BatchName).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Batch Name Already Exist");
                return ServiceResponse<BatchDto>.Return409("Batch Name Already Exist.");
            }
            var entity = _mapper.Map<Data.Batch>(request);
            //entity.Id = Guid.NewGuid();
            _batchRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<BatchDto>.Return500();
            }
            return ServiceResponse<BatchDto>.ReturnResultWith200(_mapper.Map<BatchDto>(entity));
        }
    }
}