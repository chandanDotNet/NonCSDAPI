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
    public class GetMSTBPurchaseOrderQuery : IRequest<ServiceResponse<MSTBPurchaseOrderDto>>
    {
        public Guid Id { get; set; }
    }
}
