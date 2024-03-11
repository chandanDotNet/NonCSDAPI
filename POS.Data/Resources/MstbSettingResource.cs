using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class MstbSettingResource : ResourceParameters
    {
        public MstbSettingResource() : base("CreatedDate")
        {
        }        
        public Guid Id { get; set; }
        public bool IsPrimary { get; set; }

    }
}
