using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Notice.Command;

namespace POS.API.Helpers.Mapping
{
    public class NoticeProfile : Profile
    {
        public NoticeProfile()
        {
            CreateMap<Notice, NoticeDto>().ReverseMap();
            CreateMap<AddNoticeCommand, Notice>();
        }
    }
}
