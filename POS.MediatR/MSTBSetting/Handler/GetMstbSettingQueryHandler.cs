using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.MSTBSetting.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.MSTBSetting.Handler
{
    public class GetMstbSettingQueryHandler : IRequestHandler<GetMstbSettingQuery, MstbSettingList>
    {
        private readonly IMstbSettingRepository _mstbSettingRepository;
        public GetMstbSettingQueryHandler(IMstbSettingRepository mstbSettingRepository)
        {
            _mstbSettingRepository = mstbSettingRepository;
        }
        public async Task<MstbSettingList> Handle(GetMstbSettingQuery request, CancellationToken cancellationToken)
        {
            return await _mstbSettingRepository.GetMstbSettings(request.MstbSettingResource);
        }
    }
}
