using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class MstbSettingDto
    {
        public int Id { get; set; }
        public DateOnly FromMstbDate { get; set; }
        public DateOnly ToMstbDate { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
