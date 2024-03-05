using MediatR;
using POS.Data.Dto;
using POS.Data.Dto.SalesOrder;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Commands
{
    public class GetSalesOrderItemsCategoryReportCommand : IRequest<List<SalesOrderItemCategoryDto>>
    {
        public SalesOrderResource SalesOrderResource { get; set; }
    }
}
