using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class YearProfile : Profile
    {
        public YearProfile()
        {
            CreateMap<Year, YearDto>().ReverseMap();
        }
    }
}
