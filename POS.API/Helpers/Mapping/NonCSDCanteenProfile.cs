using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.NonCSDCanteen.Command;

namespace POS.API.Helpers.Mapping
{
    public class NonCSDCanteenProfile : Profile
    {
        public NonCSDCanteenProfile()
        {
            CreateMap<NonCSDCanteen, NonCSDCanteenDto>().ReverseMap();
            //CreateMap<AddNonCSDCanteenCommand, NonCSDCanteen>();
            //CreateMap<UpdateCityCommand, City>();
        }
    }
}
