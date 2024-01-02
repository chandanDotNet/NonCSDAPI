using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class HelpAndSupportProfile : Profile
    {
        public HelpAndSupportProfile()
        {
            CreateMap<HelpAndSupport, HelpAndSupportDto>().ReverseMap();
            //CreateMap<AddProductCategoryCommand, ProductCategory>();
            //CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
