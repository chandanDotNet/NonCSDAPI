﻿using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Category.Commands
{
    public class UpdateProductCategoryCommand
          : IRequest<ServiceResponse<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public string ProductCategoryUrl { get; set; }
        public bool IsImageChanged { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
        
    }
}
