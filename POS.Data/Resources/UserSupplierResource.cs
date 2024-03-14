using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class UserSupplierResource : ResourceParameters
    {
        public UserSupplierResource() : base("UserId")
        {

        }
        public Guid? UserId { get; set; }
    }
}
