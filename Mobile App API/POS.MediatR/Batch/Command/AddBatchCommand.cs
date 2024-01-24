using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Batch.Command
{
    public class AddBatchCommand : IRequest<ServiceResponse<BatchDto>>
    {
        public string BatchName { get; set; }

    }
}