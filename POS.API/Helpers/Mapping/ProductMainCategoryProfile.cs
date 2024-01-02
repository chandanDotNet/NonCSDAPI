using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Category.Commands;

namespace POS.API.Helpers.Mapping
{
    public class ProductMainCategoryProfile : Profile
    {
        public ProductMainCategoryProfile()
        {
            CreateMap<ProductMainCategory, ProductMainCategoryDto>().ReverseMap();
            //CreateMap<AddProductCategoryCommand, ProductCategory>();
            //CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
