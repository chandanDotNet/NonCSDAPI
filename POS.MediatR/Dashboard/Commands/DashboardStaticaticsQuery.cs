using POS.Data.Dto;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class DashboardStaticaticsQuery : IRequest<DashboardStatics>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid ProductMainCategoryId { get; set; }
    }
}
