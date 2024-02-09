using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace POS.Repository
{
    public class SalesOrderList : List<SalesOrderDto>
    {
        public IMapper _mapper { get; set; }
        public SalesOrderList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public SalesOrderList(List<SalesOrderDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<SalesOrderList> Create(IQueryable<SalesOrder> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new SalesOrderList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<SalesOrder> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<SalesOrderDto>> GetDtos(IQueryable<SalesOrder> source, int skip, int pageSize)
        {
            int SNo = skip+1;
            if (pageSize == 0)
            {
                var entities = await source
             .AsNoTracking()
             .Select(cs => new SalesOrderDto
             { 
                 
                 Id = cs.Id,
                 SOCreatedDate = cs.SOCreatedDate,
                 OrderNumber = cs.OrderNumber,
                 CustomerId = cs.CustomerId,
                 TotalAmount = cs.TotalAmount,
                 TotalDiscount = cs.TotalDiscount,
                 DeliveryStatus = cs.DeliveryStatus,
                 DeliveryDate = cs.DeliveryDate,
                 DeliveryCharges = cs.DeliveryCharges,
                 DeliveryAddressId = cs.DeliveryAddressId,
                 DeliveryAddress = cs.DeliveryAddress,
                 TotalTax = cs.TotalTax,
                 CustomerName = cs.Customer.CustomerName,
                 Status = cs.Status,
                 PaymentStatus = cs.PaymentStatus,
                 TotalPaidAmount = cs.TotalPaidAmount,
                 IsAppOrderRequest = cs.IsAppOrderRequest,
                 IsAdvanceOrderRequest = cs.IsAdvanceOrderRequest,
                 OrderDeliveryStatus = cs.OrderDeliveryStatus,
                 AssignDeliveryPerson = cs.User.FirstName + " " + cs.User.LastName,
                 AssignDeliveryPersonId = cs.UserId,
                 CounterName = cs.Counter.CounterName == null ? "App" : cs.Counter.CounterName,
                 Quantity = cs.SalesOrderItems.Sum(c => c.Quantity),
                 BillNo = cs.OrderNumber.Substring(3, cs.OrderNumber.Length),
                 SalesOrderPayments = _mapper.Map<List<SalesOrderPaymentDto>>(cs.SalesOrderPayments),
                 PaymentType= cs.PaymentType,
                 UTRNo = cs.UTRNo,
                 OfflineMode = cs.OfflineMode,
                 PaymentReturnDate = cs.PaymentReturnDate,
                 PaymentReturnStatus = cs.PaymentReturnStatus,
                 MobileNo = cs.Customer.MobileNo,
                 StatusType = cs.StatusType,
                 CancelReason = cs.CancelReason,
                 ProductMainCategoryId = cs.ProductMainCategoryId
             })
             .ToListAsync();
                for (int i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    if (entity.SalesOrderPayments != null)
                    {
                        var paymentList = entities[i].SalesOrderPayments.Where(x => x.SalesOrderId == entities[i].Id);
                        foreach (var item in paymentList)
                        {
                            if (paymentList.Count() > 1)
                            {
                                entities[i].PaymentMethod += //Enum.GetName(item.PaymentMethod) + ",";
                                    (item.PaymentMethod == PaymentMethod.Cash ? "Cash" :
                                    item.PaymentMethod == PaymentMethod.Cheque ? "Chq" :
                                    item.PaymentMethod == PaymentMethod.CreditCard ? "CC" :
                                    item.PaymentMethod == PaymentMethod.COD ? "COD" :
                                    item.PaymentMethod == PaymentMethod.PaytmAndOnlinePayment ? "Paytm" :
                                    item.PaymentMethod == PaymentMethod.Other ? "Oth" : "") + ",";
                            }
                            else
                            {
                                entities[i].PaymentMethod = //Enum.GetName(item.PaymentMethod);
                                    (item.PaymentMethod == PaymentMethod.Cash ? "Cash" :
                                    item.PaymentMethod == PaymentMethod.Cheque ? "Chq" :
                                    item.PaymentMethod == PaymentMethod.CreditCard ? "CC" :
                                    item.PaymentMethod == PaymentMethod.COD ? "COD" :
                                    item.PaymentMethod == PaymentMethod.PaytmAndOnlinePayment ? "Paytm" :
                                    item.PaymentMethod == PaymentMethod.Other ? "Oth" : "");
                            }
                        }
                    }
                }
                entities.ForEach(x => x.SNo = SNo++);
                return entities;
            }
            else
            {
                
                var entities = await source
              .Skip(skip)
              .Take(pageSize)
              .AsNoTracking()
              .Select((cs) => new SalesOrderDto
              {
                
                  Id = cs.Id,
                  SOCreatedDate = cs.SOCreatedDate,
                  OrderNumber = cs.OrderNumber,
                  CustomerId = cs.CustomerId,
                  TotalAmount = cs.TotalAmount,
                  TotalDiscount = cs.TotalDiscount,
                  DeliveryStatus = cs.DeliveryStatus,
                  DeliveryDate = cs.DeliveryDate,
                  DeliveryCharges = cs.DeliveryCharges,
                  DeliveryAddressId = cs.DeliveryAddressId,
                  DeliveryAddress = cs.DeliveryAddress,
                  TotalTax = cs.TotalTax,
                  CustomerName = cs.Customer.CustomerName,
                  Status = cs.Status,
                  PaymentStatus = cs.PaymentStatus,
                  TotalPaidAmount = cs.TotalPaidAmount,
                  IsAppOrderRequest = cs.IsAppOrderRequest,
                  IsAdvanceOrderRequest = cs.IsAdvanceOrderRequest,
                  OrderDeliveryStatus = cs.OrderDeliveryStatus,
                  AssignDeliveryPerson = cs.User.FirstName + " " + cs.User.LastName,
                  AssignDeliveryPersonId = cs.UserId,
                  //DeliveryAddress = cs.DeliveryAddress
                  CounterName = cs.Counter.CounterName == null ? "App" : cs.Counter.CounterName,
                  Quantity = cs.SalesOrderItems.Sum(c => c.Quantity),
                  BillNo = cs.OrderNumber.Substring(3, cs.OrderNumber.Length),
                  SalesOrderPayments = _mapper.Map<List<SalesOrderPaymentDto>>(cs.SalesOrderPayments),
                  PaymentType = cs.PaymentType,
                  UTRNo = cs.UTRNo,
                  OfflineMode = cs.OfflineMode,
                  PaymentReturnDate = cs.PaymentReturnDate,
                  PaymentReturnStatus = cs.PaymentReturnStatus,
                  MobileNo = cs.Customer.MobileNo,
                  StatusType = cs.StatusType,
                  CancelReason = cs.CancelReason,
                  ProductMainCategoryId = cs.ProductMainCategoryId
              })
              .ToListAsync();
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].CounterName =="App")
                    {

                        if (entities[i].ProductMainCategoryId ==(new Guid("AFC982AC-5E05-4633-99FB-08DBE76CDB9B")))
                        {
                            entities[i].CounterName = "App(Needs)";
                        }else if(entities[i].ProductMainCategoryId == (new Guid("06C71507-B6DE-4D59-DE84-08DBEB3C9568")))
                        {
                            entities[i].CounterName = "App(Bakery)";
                        }
                    }
                    var entity = entities[i];
                    if (entity.SalesOrderPayments != null)
                    {
                        var paymentList = entities[i].SalesOrderPayments.Where(x => x.SalesOrderId == entities[i].Id);
                        foreach (var item in paymentList)
                        {
                            if (paymentList.Count() > 1)
                            {
                                entities[i].PaymentMethod += //Enum.GetName(item.PaymentMethod) + ",";
                                    (item.PaymentMethod == PaymentMethod.Cash ? "Cash" :
                                    item.PaymentMethod == PaymentMethod.Cheque ? "Chq" :
                                    item.PaymentMethod == PaymentMethod.CreditCard ? "CC" :
                                    item.PaymentMethod == PaymentMethod.COD ? "COD" :
                                    item.PaymentMethod == PaymentMethod.PaytmAndOnlinePayment ? "Paytm" :
                                    item.PaymentMethod == PaymentMethod.Other ? "Oth" : "") + ",";
                            }
                            else
                            {
                                entities[i].PaymentMethod = //Enum.GetName(item.PaymentMethod);
                                    (item.PaymentMethod == PaymentMethod.Cash ? "Cash" :
                                    item.PaymentMethod == PaymentMethod.Cheque ? "Chq" :
                                    item.PaymentMethod == PaymentMethod.CreditCard ? "CC" :
                                    item.PaymentMethod == PaymentMethod.COD ? "COD" :
                                    item.PaymentMethod == PaymentMethod.PaytmAndOnlinePayment ? "Paytm" :
                                    item.PaymentMethod == PaymentMethod.Other ? "Oth" : "");
                            }
                        }
                    }
                }
                entities.ForEach(x => x.SNo = SNo++);
                //return entities.OrderBy(x=>x.SOCreatedDate).ToList();
                return entities;
            }

        }
    }
}
