using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            CreateMap<FAQ, FAQDto>().ReverseMap();
            //CreateMap<AddProductCategoryCommand, ProductCategory>();
            //CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
