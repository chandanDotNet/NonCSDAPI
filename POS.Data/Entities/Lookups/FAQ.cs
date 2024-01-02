using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class FAQ : BaseEntity
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
