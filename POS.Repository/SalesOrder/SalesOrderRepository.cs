using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class SalesOrderRepository
        : GenericRepository<SalesOrder, POSDbContext>, ISalesOrderRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public SalesOrderRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService, IMapper mapper) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }
        public async Task<SalesOrderList> GetAllSalesOrders(SalesOrderResource salesOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Customer, u => u.User, k => k.Counter
            //, r => r.User.Counter,
            //    cs => cs.SalesOrderItems, cp => cp.SalesOrderPayments 
                ).ApplySort(salesOrderResource.OrderBy,
                 _propertyMappingService.GetPropertyMapping<SalesOrderDto, SalesOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsSalesOrderRequest == salesOrderResource.IsSalesOrderRequest);

            if (salesOrderResource.Status != SalesOrderStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == salesOrderResource.Status);
            }

            if (salesOrderResource.CustomerId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CustomerId == salesOrderResource.CustomerId);
            }

            if (salesOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrderItems.Any(c => c.ProductId == salesOrderResource.ProductId));
            }

            if (salesOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate >= new DateTime(salesOrderResource.FromDate.Value.Year, salesOrderResource.FromDate.Value.Month, salesOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (salesOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate <= new DateTime(salesOrderResource.ToDate.Value.Year, salesOrderResource.ToDate.Value.Month, salesOrderResource.ToDate.Value.Day, 23, 59, 59));
            }


            if (!string.IsNullOrWhiteSpace(salesOrderResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Customer.CustomerName == salesOrderResource.CustomerName.GetUnescapestring());
            }
            if (!string.IsNullOrWhiteSpace(salesOrderResource.MobileNo))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Customer.MobileNo == salesOrderResource.MobileNo.GetUnescapestring());
            }

            if (salesOrderResource.SOCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.SOCreatedDate >= new DateTime(salesOrderResource.SOCreatedDate.Value.Year, salesOrderResource.SOCreatedDate.Value.Month, salesOrderResource.SOCreatedDate.Value.Day, 0, 0, 1));

                collectionBeforePaging = collectionBeforePaging
                .Where(a => a.SOCreatedDate <= new DateTime(salesOrderResource.SOCreatedDate.Value.Year, salesOrderResource.SOCreatedDate.Value.Month, salesOrderResource.SOCreatedDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(salesOrderResource.OrderNumber))
            {
                var orderNumber = salesOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));

            }
            if (!string.IsNullOrWhiteSpace(salesOrderResource.OrderDeliveryStatus))
            {
                var orderDeliveryStatus = salesOrderResource.OrderDeliveryStatus;
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderDeliveryStatus, $"%{orderDeliveryStatus}%"));

            }

            if(salesOrderResource.IsAppOrderRequest.HasValue)
            {

            }
            else
            {
                salesOrderResource.IsAppOrderRequest = false;
            }

            if (salesOrderResource.IsAppOrderRequest.HasValue)
            {
                if (salesOrderResource.CounterName == "all")
                {

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(salesOrderResource.CounterName))
                    {
                        collectionBeforePaging = collectionBeforePaging
                            .Where(a => a.IsAppOrderRequest == salesOrderResource.IsAppOrderRequest &&
                            a.Counter.CounterName == salesOrderResource.CounterName);
                    }
                    else
                    {
                        collectionBeforePaging = collectionBeforePaging
                        .Where(a => a.IsAppOrderRequest == salesOrderResource.IsAppOrderRequest);
                    }

                }
            }
            



            //if (salesOrderResource.IsAppOrderRequest.HasValue && salesOrderResource.CounterName != "all")
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.IsAppOrderRequest == salesOrderResource.IsAppOrderRequest &&
            //        a.Counter.CounterName == salesOrderResource.CounterName);

            //    //if (salesOrderResource.IsAppOrderRequest == true)
            //    //{
            //    //    salesOrderResource.CounterName = string.Empty;
            //    //}
            //}

            if (salesOrderResource.IsAdvanceOrderRequest.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.IsAdvanceOrderRequest == salesOrderResource.IsAdvanceOrderRequest);
            }

            //if (!string.IsNullOrWhiteSpace(salesOrderResource.CounterName))
            //{
            //    if (salesOrderResource.CounterName != "all")
            //    {
            //        if (salesOrderResource.CounterName == "App")
            //        {
            //            collectionBeforePaging = collectionBeforePaging
            //       .Where(a => a.IsAppOrderRequest == true);

            //        }
            //        else
            //        {
            //            collectionBeforePaging = collectionBeforePaging
            //            .Where(a => a.Counter.CounterName == salesOrderResource.CounterName);
            //        }
            //    }
            //    else
            //    {
            //        collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.IsAppOrderRequest == false);
            //    }
            //}

            if (salesOrderResource.ProductMainCategoryId.HasValue)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.ProductMainCategoryId == salesOrderResource.ProductMainCategoryId);
            }

            var salesOrders = new SalesOrderList(_mapper);
            return await salesOrders
                .Create(collectionBeforePaging, salesOrderResource.Skip, salesOrderResource.PageSize);
        }

        public async Task<SalesOrderList> GetAllCancelSalesOrders(SalesOrderResource salesOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Customer, u => u.User, k => k.Counter
            //, r => r.User.Counter,
            //    cs => cs.SalesOrderItems, cp => cp.SalesOrderPayments
                ).Where(x => x.IsDeleted == true).ApplySort(salesOrderResource.OrderBy,
                 _propertyMappingService.GetPropertyMapping<SalesOrderDto, SalesOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsSalesOrderRequest == salesOrderResource.IsSalesOrderRequest);

            if (salesOrderResource.Status != SalesOrderStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == salesOrderResource.Status);
            }

            if (salesOrderResource.CustomerId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CustomerId == salesOrderResource.CustomerId);
            }

            if (salesOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrderItems.Any(c => c.ProductId == salesOrderResource.ProductId));
            }

            if (salesOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate >= new DateTime(salesOrderResource.FromDate.Value.Year, salesOrderResource.FromDate.Value.Month, salesOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (salesOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate <= new DateTime(salesOrderResource.ToDate.Value.Year, salesOrderResource.ToDate.Value.Month, salesOrderResource.ToDate.Value.Day, 23, 59, 59));
            }


            if (!string.IsNullOrWhiteSpace(salesOrderResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Customer.CustomerName == salesOrderResource.CustomerName.GetUnescapestring());
            }
            if (!string.IsNullOrWhiteSpace(salesOrderResource.MobileNo))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Customer.MobileNo == salesOrderResource.MobileNo.GetUnescapestring());
            }

            if (salesOrderResource.SOCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.SOCreatedDate >= new DateTime(salesOrderResource.SOCreatedDate.Value.Year, salesOrderResource.SOCreatedDate.Value.Month, salesOrderResource.SOCreatedDate.Value.Day, 0, 0, 1));

                collectionBeforePaging = collectionBeforePaging
                .Where(a => a.SOCreatedDate <= new DateTime(salesOrderResource.SOCreatedDate.Value.Year, salesOrderResource.SOCreatedDate.Value.Month, salesOrderResource.SOCreatedDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(salesOrderResource.OrderNumber))
            {
                var orderNumber = salesOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));

            }
            if (!string.IsNullOrWhiteSpace(salesOrderResource.OrderDeliveryStatus))
            {
                var orderDeliveryStatus = salesOrderResource.OrderDeliveryStatus;
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderDeliveryStatus, $"%{orderDeliveryStatus}%"));

            }

            if (salesOrderResource.IsAppOrderRequest.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.IsAppOrderRequest == salesOrderResource.IsAppOrderRequest);

                if (salesOrderResource.IsAppOrderRequest == true)
                {
                    salesOrderResource.CounterName = string.Empty;
                }
            }

            if (salesOrderResource.IsAdvanceOrderRequest.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.IsAdvanceOrderRequest == salesOrderResource.IsAdvanceOrderRequest);
            }

            if (!string.IsNullOrWhiteSpace(salesOrderResource.CounterName))
            {
                if (salesOrderResource.CounterName != "all")
                {
                    if (salesOrderResource.CounterName == "App")
                    {
                        collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.IsAppOrderRequest == true);

                    }
                    else
                    {
                        collectionBeforePaging = collectionBeforePaging
                        .Where(a => a.Counter.CounterName == salesOrderResource.CounterName);
                    }
                }
                else
                {
                    collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.IsAppOrderRequest == false);
                }
            }

            if (salesOrderResource.ProductMainCategoryId.HasValue)
            {
                // trim & ignore casing
                collectionBeforePaging = collectionBeforePaging
                   .Where(a => a.ProductMainCategoryId == salesOrderResource.ProductMainCategoryId);
            }

            var salesOrders = new SalesOrderList(_mapper);
            return await salesOrders
                .Create(collectionBeforePaging, salesOrderResource.Skip, salesOrderResource.PageSize);
        }
    }
}

