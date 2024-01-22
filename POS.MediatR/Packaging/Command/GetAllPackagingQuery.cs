using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllPackagingQuery : IRequest<List<PackagingDto>>
    {
        public Guid? Id { get; set; }
        public bool IsDropDown { get; set; } = false;
    }
}
