using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Banner.Command;
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
    public class GetAllNoticeCommandHandler : IRequestHandler<GetAllNoticeCommand, List<NoticeDto>>
    {
        private readonly INoticeRepository _noticeRepository;
        private readonly IMapper _mapper;

        public GetAllNoticeCommandHandler(
           INoticeRepository noticeRepository,
            IMapper mapper)
        {
            _noticeRepository = noticeRepository;
            _mapper = mapper;
        }

        public async Task<List<NoticeDto>> Handle(GetAllNoticeCommand request, CancellationToken cancellationToken)
        {
            var entities = await _noticeRepository.All
                .Select(c => new NoticeDto
                {
                    Id = c.Id,
                    Text = c.Text
                }).ToListAsync();
            return entities;
        }
    }
}