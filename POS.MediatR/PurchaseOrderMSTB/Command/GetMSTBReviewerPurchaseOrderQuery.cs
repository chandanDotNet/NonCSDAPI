using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetMSTBReviewerPurchaseOrderQuery : IRequest<ServiceResponse<MSTBPurchaseOrderDto>>
    {
        public Guid SupplierId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string? ProductName { get; set; }
    }
}

