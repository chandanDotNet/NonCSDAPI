using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Commands
{
    public class SyncSalesOrderAmountCommand : IRequest<ServiceResponse<SalesOrderDto>>
    {

        public List<SalesOrderDto> SalesOrder { get; set; }
    }
}
