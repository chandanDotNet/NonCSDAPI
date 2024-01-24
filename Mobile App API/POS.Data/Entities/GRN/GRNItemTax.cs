using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities.GRN
{
    public class GRNItemTax
    {

        public Guid Id { get; set; }
        public Guid GRNItemId { get; set; }
        [ForeignKey("GRNItemId")]
        public GRNItem GRNItem { get; set; }
        public Guid TaxId { get; set; }
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }
    }
}
