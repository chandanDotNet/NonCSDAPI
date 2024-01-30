using POS.Data.Dto;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class DashboardStaticaticsQuery : IRequest<DashboardStatics>
    {
        public Guid ProductMainCategoryId { get; set; }
    }
}
