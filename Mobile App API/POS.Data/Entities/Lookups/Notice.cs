using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class Notice : BaseEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
