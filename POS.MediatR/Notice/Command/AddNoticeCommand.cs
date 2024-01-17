using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Notice.Command
{
    public class AddNoticeCommand : IRequest<ServiceResponse<NoticeDto>>
    {
        public string Text { get; set; }
    }
}
