using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class YearDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool DefaultYear { get; set; }
    }
}
