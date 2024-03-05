using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetNewMSTBPurchaseOrderNumberQuery : IRequest<string>
    {
        public bool isMSTBPurchaseOrder { get; set; }
    }
}
