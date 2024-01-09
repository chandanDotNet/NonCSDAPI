using MediatR;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class SearchManufacturerQuery : IRequest<ManufacturerList>
    {
        //public string SearchQuery { get; set; }
        //public int PageSize { get; set; } = 10;
        public ManufacturerResource ManufacturerResource { get; set; }
    }
}
