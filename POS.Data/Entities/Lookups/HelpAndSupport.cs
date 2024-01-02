using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class HelpAndSupport : BaseEntity
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
    }
}
