﻿using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class ShopHolidayProfile : Profile
    {
        public ShopHolidayProfile()
        {
            CreateMap<ShopHoliday, ShopHolidayDto>().ReverseMap();
            //CreateMap<AddShopHolidayCommand, ShopHoliday>();
        }
    }
}
