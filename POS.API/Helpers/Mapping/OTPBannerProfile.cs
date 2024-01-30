using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class OTPBannerProfile : Profile
    {
        public OTPBannerProfile()
        {
            CreateMap<OTPBanner, OTPBannerDto>().ReverseMap();
            CreateMap<AddOTPBannerCommand, OTPBanner>();
        }
    }
}
