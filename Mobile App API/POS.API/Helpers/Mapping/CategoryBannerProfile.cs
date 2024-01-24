using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class CategoryBannerProfile : Profile
    {
        public CategoryBannerProfile()
        {
            CreateMap<CategoryBanner, CategoryBannerDto>().ReverseMap();
            CreateMap<AddCategoryBannerCommand, CategoryBanner>();
        }
    }
}