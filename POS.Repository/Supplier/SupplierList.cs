using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class SupplierList : List<SupplierDto>
    {
        public IMapper _mapper { get; set; }
        public SupplierList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public SupplierList(List<SupplierDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<SupplierList> Create(IQueryable<Supplier> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new SupplierList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Supplier> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public int GetAllData(int totalCount, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = totalCount;
            }
            return pageSize;
        }

        public async Task<List<SupplierDto>> GetDtos(IQueryable<Supplier> source, int skip, int pageSize)
        {
            int SNo = skip;
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new SupplierDto
                {
                   
                    Id = c.Id,
                    BusinessType = c.BusinessType,
                    ContactPerson = c.ContactPerson,
                    Description = c.Description,
                    Fax = c.Fax,
                    SupplierName = c.SupplierName,
                    Website = c.Website,
                    MobileNo = c.MobileNo,
                    PhoneNo = c.PhoneNo,
                    Country = c.SupplierAddress.CountryName,
                    Email = c.Email,
                    PANNo=c.PANNo,
                    GSTNo=c.GSTNo,
                    MSME = c.MSME,
                    UdyogAadhar = c.UdyogAadhar,
                    Pin=c.Pin
                    
                }).ToListAsync();
            entities.ForEach(x => x.SNo = SNo++);
            return _mapper.Map<List<SupplierDto>>(entities);
        }
    }
}
