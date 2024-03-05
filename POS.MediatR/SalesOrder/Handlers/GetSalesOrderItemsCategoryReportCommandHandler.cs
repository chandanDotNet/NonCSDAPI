using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data.Dto.SalesOrder;
using POS.Data.Resources;
using POS.MediatR.Dashboard.Commands;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Handlers
{
    public class GetSalesOrderItemsCategoryReportCommandHandler : IRequestHandler<GetSalesOrderItemsCategoryReportCommand, List<SalesOrderItemCategoryDto>>
    {
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;

        public GetSalesOrderItemsCategoryReportCommandHandler(ISalesOrderItemRepository salesOrderItemRepository)
        {
            _salesOrderItemRepository = salesOrderItemRepository;
        }

        public async Task<List<SalesOrderItemCategoryDto>> Handle(GetSalesOrderItemsCategoryReportCommand request, CancellationToken cancellationToken)
        {

            var itemsQuery = _salesOrderItemRepository.AllIncluding(c => c.SalesOrder, cs => cs.Product, cs => cs.Product.ProductCategory);

            if (request.SalesOrderResource.FromDate.HasValue)
            {
                itemsQuery = itemsQuery
                    .Where(a => a.SalesOrder.SOCreatedDate >= new DateTime(request.SalesOrderResource.FromDate.Value.Year, request.SalesOrderResource.FromDate.Value.Month, request.SalesOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (request.SalesOrderResource.ToDate.HasValue)
            {
                itemsQuery = itemsQuery
                    .Where(a => a.SalesOrder.SOCreatedDate <= new DateTime(request.SalesOrderResource.ToDate.Value.Year, request.SalesOrderResource.ToDate.Value.Month, request.SalesOrderResource.ToDate.Value.Day, 23, 59, 59));
            }
            if (!string.IsNullOrEmpty(request.SalesOrderResource.CategoryType))
            {
                if (request.SalesOrderResource.CategoryType == "Vegetables")
                {
                    itemsQuery = itemsQuery
                        .Where(a => a.Product.CategoryId == new Guid("4015D064-3CFF-4459-9E9B-5DA260598447"));
                }
                else
                {
                    itemsQuery = itemsQuery
                        .Where(a => a.Product.CategoryId != new Guid("4015D064-3CFF-4459-9E9B-5DA260598447"));

                    itemsQuery = itemsQuery
                        .Where(a => a.Product.BrandId != new Guid("A9F6308B-C7F7-42A2-0AFD-08DC0694AD26"));
                }
            }

            //if (request.SalesOrderResource.ProductCategoryId.HasValue)
            //{
            //    itemsQuery = itemsQuery
            //        .Where(a => a.Product.CategoryId == request.SalesOrderResource.ProductCategoryId);
            //}

            if (request.SalesOrderResource.ProductMainCategoryId.HasValue)
            {
                itemsQuery = itemsQuery
                    .Where(a => a.Product.ProductCategory.ProductMainCategoryId == request.SalesOrderResource.ProductMainCategoryId);
            }

            var groupedCollection = itemsQuery.GroupBy(c => new { c.Product.Name, c.Product.Barcode });

            int SNo = 1;
            var ItemCategory = await groupedCollection
               .Select(cs => new SalesOrderItemCategoryDto
               {
                   ProductName = cs.FirstOrDefault().Product.Name,
                   Barcode = cs.FirstOrDefault().Product.Barcode,
                   PurchasePrice = cs.FirstOrDefault().PurchasePrice.Value,
                   Rate = cs.FirstOrDefault().UnitPrice,
                   Quantity = cs.Sum(item => item.Quantity),
                   TotalPurchasePrice = Math.Round(cs.Sum(item => item.TotalPurPrice.Value), MidpointRounding.AwayFromZero).ToString("0.00"),
                   TotalSalesPrice = cs.Sum(item => item.TotalSalesPrice.Value)
               })
               .OrderBy(c => c.ProductName)
               .ToListAsync();
            ItemCategory.ForEach(x => x.SlNo = SNo++);

            return ItemCategory;
        }
    }
}
