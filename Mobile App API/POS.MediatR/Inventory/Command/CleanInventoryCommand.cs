using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Inventory.Command
{
    public class CleanInventoryCommand : IRequest<ServiceResponse<bool>>
    {
    }
}
