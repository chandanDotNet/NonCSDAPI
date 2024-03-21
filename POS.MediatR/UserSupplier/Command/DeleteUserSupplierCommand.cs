using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.UserSupplier.Command
{
    public class DeleteUserSupplierCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid UserId { get; set; }
    }
}