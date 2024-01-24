using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Commands
{
    public class AddCounterCommand : IRequest<ServiceResponse<CounterDto>>
    {
        public string CounterName { get; set; }
        public float Latitude { get; set; }
        public float Longitutde { get; set; }
        public string? CounterNo { get; set; }
    }
}
