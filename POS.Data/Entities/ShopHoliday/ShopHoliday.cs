using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class ShopHoliday : BaseEntity
    {
        public Guid Id { get; set; }
        public string HolidayName { get; set; }        
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string NotificationMessage { get; set; }
        public bool IsOpenClose { get; set; }
        public bool IsAdvanceOrder { get; set; }
    }
}
