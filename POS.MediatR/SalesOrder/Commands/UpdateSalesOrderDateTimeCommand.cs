using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Commands
{
    public class UpdateSalesOrderDateTimeCommand : IRequest<ServiceResponse<SalesOrderDto>>
    {
        public DateTime SOCreatedDate { get; set; }        
        public DateTime DeliveryDate { get; set; }
        public List<SalesOrderDto> SalesOrder { get; set; }

    }
}
