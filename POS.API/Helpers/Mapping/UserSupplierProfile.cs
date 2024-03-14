using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.UserSupplier.Command;

namespace POS.API.Helpers.Mapping
{
    public class UserSupplierProfile : Profile
    {
        public UserSupplierProfile()
        {
            CreateMap<UserSupplier, UserSupplierDto>().ReverseMap();
            CreateMap<AddUserSupplierCommand, UserSupplier>();
        }
    }
}