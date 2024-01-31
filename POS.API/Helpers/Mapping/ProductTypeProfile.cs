using AutoMapper;
using POS.Data.Dto;
using POS.Data;

namespace POS.API.Helpers.Mapping
{
    public class ProductTypeProfile : Profile
    {
        public ProductTypeProfile()
        {
            CreateMap<ProductType, ProductTypeDto>().ReverseMap();

        }
    }
}
