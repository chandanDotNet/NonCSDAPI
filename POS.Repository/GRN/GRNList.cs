using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Dto.GRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class GRNList : List<GRNDto>
    {

        public GRNList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public GRNList(List<GRNDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<GRNList> Create(IQueryable<GRN> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new GRNList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<GRN> source)
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

        public async Task<List<GRNDto>> GetDtos(IQueryable<GRN> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new GRNDto
                    {
                        Id = cs.Id,
                        POCreatedDate = cs.POCreatedDate,
                        GRNNumber = cs.GRNNumber,
                        SupplierId = cs.SupplierId,
                        TotalAmount = cs.TotalAmount,
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
                        Note = cs.Note
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
             .Select(cs => new GRNDto
             {
                 Id = cs.Id,
                 POCreatedDate = cs.POCreatedDate,
                 GRNNumber = cs.GRNNumber,
                 SupplierId = cs.SupplierId,
                 TotalAmount = cs.TotalAmount,
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
                 Note = cs.Note
             })
             .ToListAsync();
                return entities;
            }
        }


    }
}
