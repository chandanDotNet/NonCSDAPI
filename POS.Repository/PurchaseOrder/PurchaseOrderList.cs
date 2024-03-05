using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Data.Entities;

namespace POS.Repository
{
    public class PurchaseOrderList : List<PurchaseOrderDto>
    {
       
        public PurchaseOrderList()
        {
            
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PurchaseOrderList(List<PurchaseOrderDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PurchaseOrderList> Create(IQueryable<PurchaseOrder> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new PurchaseOrderList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PurchaseOrder> source)
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

        public async Task<List<PurchaseOrderDto>> GetDtos(IQueryable<PurchaseOrder> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new PurchaseOrderDto
                    {
                        Id = cs.Id,
                        POCreatedDate = cs.POCreatedDate,
                        OrderNumber = cs.OrderNumber,
                        InvoiceNo = cs.InvoiceNo,
                        SupplierId = cs.SupplierId,
                        TotalAmount = cs.TotalAmount,
                        TotalSaleAmount = cs.TotalSaleAmount,
                        TotalDiscount = cs.TotalDiscount,
                        DeliveryStatus = cs.DeliveryStatus,
                        DeliveryDate = cs.DeliveryDate,
                        TotalTax = cs.TotalTax,
                        SupplierName = cs.Supplier.SupplierName,
                        Status = cs.Status,
                        PaymentStatus = cs.PaymentStatus,
                        TotalPaidAmount = cs.TotalPaidAmount,
                        TermAndCondition = cs.TermAndCondition,
                        IsPurchaseOrderRequest = cs.IsPurchaseOrderRequest,
                        Note = cs.Note,
                        PurchasePaymentType = cs.PurchasePaymentType,
                        PurchaseOrderPaymentStatus = cs.PurchaseOrderPaymentStatus,
                        PurchaseOrderReturnType = cs.PurchaseOrderReturnType,
                        BatchNo = cs.BatchNo,
                        Year= cs.Year,
                        Month= cs.Month,
                        IsMSTBGRN = cs.IsMSTBGRN.Value,
                        IsAppMSTB = cs.IsAppMSTB.Value,
                        PurchaseOrderItems = cs.PurchaseOrderItems.Select(c => new PurchaseOrderItemDto
                        {
                            Id = c.Id,
                            ProductId = c.ProductId,
                            Quantity = c.Quantity,
                            Status = c.Status,
                            SalesPrice = c.SalesPrice,
                            UnitPrice = c.UnitPrice,
                        }).ToList(),
                        TotalReturnAmount = cs.PurchaseOrderItems.Where(c => (c.Status == PurchaseSaleItemStatusEnum.Return) && c.PurchaseOrderId == cs.Id).Sum(c => c.UnitPrice* c.Quantity)
                        //TotalReturnAmount = cs.PurchaseOrderItems.Select(x => new PurchaseOrderItemDto { SalesPrice=x.SalesPrice }).Where(x => x.Status == Data.Entities.PurchaseSaleItemStatusEnum.Return).Sum(x => Convert.ToDecimal(x.SalesPrice))
                        //TotalReturnAmount = cs.PurchaseOrderItems.Select(x => new PurchaseOrderItemDto { SalesPrice=(x.SalesPrice) }).Where(x => x.Status == Data.Entities.PurchaseSaleItemStatusEnum.Return).Sum(x => Convert.ToDecimal(x.SalesPrice))
                    })
                    .ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(cs => new PurchaseOrderDto
             {
                 Id = cs.Id,
                 POCreatedDate = cs.POCreatedDate,
                 OrderNumber = cs.OrderNumber,
                 InvoiceNo = cs.InvoiceNo,
                 SupplierId = cs.SupplierId,
                 TotalAmount = cs.TotalAmount,
                 TotalSaleAmount = cs.TotalSaleAmount,
                 TotalDiscount = cs.TotalDiscount,
                 DeliveryStatus = cs.DeliveryStatus,
                 DeliveryDate = cs.DeliveryDate,
                 TotalTax = cs.TotalTax,
                 SupplierName = cs.Supplier.SupplierName,
                 Status = cs.Status,
                 PaymentStatus = cs.PaymentStatus,
                 TotalPaidAmount = cs.TotalPaidAmount,
                 IsPurchaseOrderRequest = cs.IsPurchaseOrderRequest,
                 TermAndCondition = cs.TermAndCondition,
                 Note = cs.Note,
                 PurchasePaymentType = cs.PurchasePaymentType,
                 //PurchaseOrderItems= _mapper.Map<PurchaseOrderItemDto>(cs.PurchaseOrderItems)
                 PurchaseOrderPaymentStatus = cs.PurchaseOrderPaymentStatus,
                 PurchaseOrderReturnType = cs.PurchaseOrderReturnType,
                 BatchNo = cs.BatchNo,
                 Year = cs.Year,
                 Month = cs.Month,
                 IsMSTBGRN = cs.IsMSTBGRN.Value,
                 IsAppMSTB = cs.IsAppMSTB.Value,
                 PurchaseOrderItems = cs.PurchaseOrderItems.Select(c => new PurchaseOrderItemDto
                 {
                     Id = c.Id,
                     ProductId = c.ProductId,
                     Quantity = c.Quantity,
                     Status = c.Status,
                     SalesPrice = c.SalesPrice,
                     UnitPrice = c.UnitPrice,
                 }).ToList(),
                 TotalReturnAmount = cs.PurchaseOrderItems.Where(c => (c.Status == PurchaseSaleItemStatusEnum.Return) && c.PurchaseOrderId == cs.Id).Sum(c => c.UnitPrice * c.Quantity)
                 // TotalReturnAmount = cs.PurchaseOrderItems.Select(x => new PurchaseOrderItemDto { SalesPrice = x.SalesPrice }).Sum(x => Convert.ToDecimal(x.SalesPrice))
             })
             .ToListAsync();
                return entities;
            }
        }
    }
}
