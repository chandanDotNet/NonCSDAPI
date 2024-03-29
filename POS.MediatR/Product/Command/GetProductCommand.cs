﻿using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Product.Command
{
    public class GetProductCommand : IRequest<ServiceResponse<ProductDto>>
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
