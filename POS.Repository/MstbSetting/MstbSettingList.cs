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
    public class MstbSettingList : List<MstbSettingDto>
    {
        public MstbSettingList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public MstbSettingList(List<MstbSettingDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<MstbSettingList> Create(IQueryable<MstbSetting> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, 0);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new MstbSettingList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<MstbSetting> source)
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

        public async Task<List<MstbSettingDto>> GetDtos(IQueryable<MstbSetting> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new MstbSettingDto
                {
                    Id = c.Id,
                    MonthName = c.MonthName,
                    Month = c.Month,
                    Year = c.Year,
                    FromMstbDate = c.FromMstbDate,
                    ToMstbDate = c.ToMstbDate,
                    IsDefault= c.IsDefault
                }).ToListAsync();
            return entities;
        }
    }
}