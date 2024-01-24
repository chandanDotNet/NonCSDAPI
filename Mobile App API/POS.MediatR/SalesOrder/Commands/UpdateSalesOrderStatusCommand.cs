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
    public class UpdateSalesOrderStatusCommand : IRequest<ServiceResponse<SalesOrderDto>>
    {
        public Guid SalesOrderId { get; set; }
        //public string OrderNumber { get; set; }
        //public DeliveryStatus DeliveryStatus { get; set; }
        public Guid? DeliveryPersonId { get; set; }
        public string OrderDeliveryStatus { get; set; }

    }
}
