using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class AutoGenerateGRNResource : ResourceParameter
    {

        public AutoGenerateGRNResource() : base("POCreatedDate")
        {
        }

              
        public List<AutoGenerateGRNSupplierListResource> AutoGenerateGRNSupplierItems { get; set; }


    }

    public class AutoGenerateGRNSupplierListResource
    {
        public string OrderNumber { get; set; }
        public Guid SupplierId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

        public List<AutoGenerateGRNProductListResource> AutoGenerateGRNProductItems { get; set; }

    }

    public class AutoGenerateGRNProductListResource
    {
        public Guid? ProductId { get; set; }
        
        public string ProductName { get; set; }
        public Guid? UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Margin { get; set; }
        public decimal? SalesPrice { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; } = 0;

    }



}
