using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class NonCSDCanteenDto
    {
        public Guid Id { get; set; }
        public string CanteenLocationName { get; set; }
        public Guid? MainCategoryId { get; set; }
    }
}
