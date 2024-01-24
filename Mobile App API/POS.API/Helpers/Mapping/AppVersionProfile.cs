using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class AppVersionProfile : Profile
    {
        public AppVersionProfile()
        {
            CreateMap<AppVersion, AppVersionDto>().ReverseMap();
            //CreateMap<AddProductCategoryCommand, ProductCategory>();
            //CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
