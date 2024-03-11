using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CustomerAddress.Commands;
using POS.MediatR.MSTBSetting.Command;

namespace POS.API.Helpers.Mapping
{
    public class MstbSettingProfile : Profile
    {
        public MstbSettingProfile()
        {
            CreateMap<MstbSetting, MstbSettingDto>().ReverseMap();
            CreateMap<AddMstbSettingCommand, MstbSetting>();
            //CreateMap<UpdateCustomerAddressCommand, CustomerAddress>();
        }
    }
}
