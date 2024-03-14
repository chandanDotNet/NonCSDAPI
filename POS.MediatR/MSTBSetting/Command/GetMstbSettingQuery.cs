using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.MSTBSetting.Command
{
    public class GetMstbSettingQuery : IRequest<MstbSettingList>
    {
        public MstbSettingResource MstbSettingResource { get; set; }
    }
}


