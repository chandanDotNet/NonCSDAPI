﻿using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Command
{
    public class AddCategoryBannerCommand : IRequest<ServiceResponse<CategoryBannerDto>>
    {
        public string Name { get; set; }
        public string ImageUrlData { get; set; }
        public string ImageType { get; set; }
    }
}

