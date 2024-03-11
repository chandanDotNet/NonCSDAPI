using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class MstbSettingDto
    {
        public Guid Id { get; set; }
        public DateTime FromMstbDate { get; set; }
        public DateTime ToMstbDate { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsDefault { get; set; }
    }
}
