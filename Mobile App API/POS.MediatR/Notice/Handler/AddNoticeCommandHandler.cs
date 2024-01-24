using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Notice.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Notice.Handler
{
    public class AddNoticeCommandHandler : IRequestHandler<AddNoticeCommand, ServiceResponse<NoticeDto>>
    {
        private readonly INoticeRepository _noticeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddNoticeCommandHandler> _logger;

        public AddNoticeCommandHandler(
           INoticeRepository noticeRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddNoticeCommandHandler> logger
            )
        {
            _noticeRepository = noticeRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<NoticeDto>> Handle(AddNoticeCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _noticeRepository.All.FirstOrDefaultAsync();
            var entity = _mapper.Map<Data.Notice>(request);

            if (existingEntity == null)
            {                
                _noticeRepository.Add(entity);
            }
            else
            {
                existingEntity.Text = request.Text;
                _noticeRepository.Update(existingEntity);
            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<NoticeDto>.Return500();
            }

            var entityToReturn = _mapper.Map<NoticeDto>(existingEntity == null ? entity : existingEntity);

            return ServiceResponse<NoticeDto>.ReturnResultWith200(entityToReturn);
        }
    }
}