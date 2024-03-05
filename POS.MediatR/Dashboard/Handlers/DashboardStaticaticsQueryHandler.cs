using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using POS.Data.Entities;
using System;

namespace POS.MediatR.Handlers
{
    public class DashboardStaticaticsQueryHandler : IRequestHandler<DashboardStaticaticsQuery, DashboardStatics>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;

        public DashboardStaticaticsQueryHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            ISalesOrderRepository salesOrderRepository,
            IPurchaseOrderItemRepository purchaseOrderItemRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
        }
        public async Task<DashboardStatics> Handle(DashboardStaticaticsQuery request, CancellationToken cancellationToken)
        {

            var dashboardStatics = new DashboardStatics();
            decimal TotalPurchaseReturn = 0, TotalPurchase = 0, TotalSales=0, TotalSalesReturn=0; 
            Guid SuppliersId = new Guid("E7CCB254-6397-4294-5E7F-08DBFD80C92B");  //Southern Star Bakery
            if (request.ProductMainCategoryId == new Guid("06C71507-B6DE-4D59-DE84-08DBEB3C9568")) //Bakery
            {
                TotalPurchase = await _purchaseOrderRepository.All.Where(c => c.SupplierId == SuppliersId && c.Month == request.Month && c.Year == request.Year).SumAsync(c => c.TotalAmount);
                dashboardStatics.TotalPurchase = Math.Round(TotalPurchase, MidpointRounding.AwayFromZero);
                TotalPurchaseReturn = await _purchaseOrderItemRepository.AllIncluding(c => c.PurchaseOrder)
               .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return && c.PurchaseOrder.SupplierId == SuppliersId && c.PurchaseOrder.Month == request.Month && c.PurchaseOrder.Year == request.Year)
               .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);
                dashboardStatics.TotalPurchaseReturn = Math.Round(TotalPurchaseReturn, MidpointRounding.AwayFromZero);
            }
            else
            {
                TotalPurchase = await _purchaseOrderRepository.All.Where(c => c.SupplierId != SuppliersId && c.Month == request.Month && c.Year == request.Year).SumAsync(c => c.TotalAmount);
                dashboardStatics.TotalPurchase = Math.Round(TotalPurchase, MidpointRounding.AwayFromZero);
                TotalPurchaseReturn = await _purchaseOrderItemRepository.AllIncluding(c => c.PurchaseOrder)
               .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return && c.PurchaseOrder.SupplierId != SuppliersId && c.PurchaseOrder.Month == request.Month && c.PurchaseOrder.Year == request.Year)
               .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);
                dashboardStatics.TotalPurchaseReturn = Math.Round(TotalPurchaseReturn, MidpointRounding.AwayFromZero);
            }
           

            //dashboardStatics.TotalPurchase = await _purchaseOrderRepository.All.SumAsync(c => c.TotalAmount);

            TotalSales = await _salesOrderRepository.All.Where(a => a.ProductMainCategoryId== request.ProductMainCategoryId && a.SOCreatedDate.Month == request.Month && a.SOCreatedDate.Year == request.Year).SumAsync(c => c.TotalAmount);
            dashboardStatics.TotalSales = Math.Round(TotalSales, MidpointRounding.AwayFromZero);
            TotalSalesReturn = await _salesOrderItemRepository.AllIncluding(c => c.SalesOrder)
                .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return && c.SalesOrder.ProductMainCategoryId==request.ProductMainCategoryId &&  c.SalesOrder.SOCreatedDate.Month == request.Month && c.SalesOrder.SOCreatedDate.Year == request.Year)                
                .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);

            dashboardStatics.TotalSalesReturn= Math.Round(TotalSalesReturn, MidpointRounding.AwayFromZero);
            //dashboardStatics.TotalPurchaseReturn = await _purchaseOrderItemRepository.All
            //    .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return)
            //    .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);
            return dashboardStatics;
        }
    }
}
