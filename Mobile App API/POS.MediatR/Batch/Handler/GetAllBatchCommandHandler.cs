using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Batch.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Batch.Handler
{
    public class GetAllBatchCommandHandler : IRequestHandler<GetAllBatchCommand, List<BatchDto>>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMapper _mapper;

        public GetAllBatchCommandHandler(
           IBatchRepository batchRepository,
            IMapper mapper)
        {
            _batchRepository = batchRepository;
            _mapper = mapper;
        }

        public async Task<List<BatchDto>> Handle(GetAllBatchCommand request, CancellationToken cancellationToken)
        {
            var entities = await _batchRepository.All
                .Select(c => new BatchDto
                {
                    Id = c.Id,
                    BatchName = c.BatchName,
                }).ToListAsync();
            return entities;
        }
    }
}