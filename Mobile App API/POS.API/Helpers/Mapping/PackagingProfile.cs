using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class PackagingProfile : Profile
    {
        public PackagingProfile()
        {
            CreateMap<Packaging, PackagingDto>().ReverseMap();
           
        }
    }
}
