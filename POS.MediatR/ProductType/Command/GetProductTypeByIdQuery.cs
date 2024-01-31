using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.ProductType.Command
{
    public class GetProductTypeByIdQuery : IRequest<ServiceResponse<ProductTypeDto>>
    {
        public Guid Id { get; set; }
    }
}
