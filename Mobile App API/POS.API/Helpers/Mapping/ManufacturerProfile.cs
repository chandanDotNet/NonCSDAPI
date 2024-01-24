using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Manufacturer.Command;

namespace POS.API.Helpers.Mapping
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
            CreateMap<AddManufacturerCommand, Manufacturer>();
        }
    }
}
