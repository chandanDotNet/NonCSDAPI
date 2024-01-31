using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.ProductType.Command
{
    public class GetAllProductTypeQuery : IRequest<List<ProductTypeDto>>
    {
        public Guid? Id { get; set; }
        public bool IsDropDown { get; set; } = false;
    }
}

