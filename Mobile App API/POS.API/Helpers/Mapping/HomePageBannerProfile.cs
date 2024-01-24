using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class HomePageBannerProfile : Profile
    {
        public HomePageBannerProfile()
        {
            CreateMap<HomePageBanner, HomePageBannerDto>().ReverseMap();
            CreateMap<AddHomePageBannerCommand, HomePageBanner>();
        }
    }
}
