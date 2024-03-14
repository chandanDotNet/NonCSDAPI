using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class UserSupplierList : List<UserSupplierDto>
    {
        public IMapper _mapper { get; set; }
        public UserSupplierList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public UserSupplierList(List<UserSupplierDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<UserSupplierList> Create(IQueryable<UserSupplier> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new UserSupplierList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<UserSupplier> source)
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

        public async Task<List<UserSupplierDto>> GetDtos(IQueryable<UserSupplier> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new UserSupplierDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    SupplierId = c.SupplierId,

                }).ToListAsync();
            return _mapper.Map<List<UserSupplierDto>>(entities);
        }
    }
}
