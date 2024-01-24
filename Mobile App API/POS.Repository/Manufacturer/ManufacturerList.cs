using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class ManufacturerList : List<ManufacturerDto>
    {
        public IMapper _mapper { get; set; }
        public ManufacturerList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ManufacturerList(List<ManufacturerDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ManufacturerList> Create(IQueryable<Manufacturer> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ManufacturerList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Manufacturer> source)
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

        public async Task<List<ManufacturerDto>> GetDtos(IQueryable<Manufacturer> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new ManufacturerDto
                {
                    Id = c.Id,
                    ManufacturerName = c.ManufacturerName,

                }).ToListAsync();
            return _mapper.Map<List<ManufacturerDto>>(entities);
        }
    }
}