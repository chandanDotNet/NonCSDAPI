using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Inventory.Command
{
    public class AddInventoryCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid? ProductId { get; set; }
        public decimal Stock { get; set; }
        public decimal PricePerUnit { get; set; }
        public Guid UnitId { get; set; }
        public string? ProductCode { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public decimal? PurchasePrice { get; set; }
    }
}
