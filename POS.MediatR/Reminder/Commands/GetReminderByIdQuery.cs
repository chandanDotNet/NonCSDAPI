﻿using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
   public class GetReminderByIdQuery : IRequest<ServiceResponse<ReminderDto>>
    {
        public Guid Id { get; set; }
    }
}
