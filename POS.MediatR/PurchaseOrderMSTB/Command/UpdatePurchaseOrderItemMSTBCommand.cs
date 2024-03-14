using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderMSTB.Command
{
    public class UpdatePurchaseOrderItemMSTBCommand : IRequest<ServiceResponse<MSTBPurchaseOrderItemDto>>
    {
        public Guid Id { get; set; }
        public bool IsCheck { get; set; }
        public decimal Margin { get; set; }
        public decimal SalesPrice { get; set; }        
        public decimal Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Surplus { get; set; }
        public decimal Difference { get; set; }
        public bool? Approved { get; set; }
        public string? UserType { get; set; }
        //public string StreetDetails { get; set; }
        //public string LandMark { get; set; }        
    }
}
