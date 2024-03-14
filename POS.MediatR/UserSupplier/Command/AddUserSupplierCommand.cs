using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.UserSupplier.Command
{
    public class AddUserSupplierCommand : IRequest<ServiceResponse<UserSupplierDto>>
    {
        public List<UserSupplierDto> UserSuppliers { get; set; }
    }
}
