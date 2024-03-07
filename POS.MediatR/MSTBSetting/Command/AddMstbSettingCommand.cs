using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.MSTBSetting.Command
{
    public class AddMstbSettingCommand : IRequest<ServiceResponse<MstbSettingDto>>
    {
        public DateTime FromMstbDate { get; set; }
        public DateTime ToMstbDate { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}