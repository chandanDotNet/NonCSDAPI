using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using POS.Helper;

namespace POS.Repository
{
    public class BrandList : List<BrandDto>
    {
        public IMapper _mapper { get; set; }
        private PathHelper _pathHelper { get; set; }
        public BrandList(IMapper mapper, PathHelper pathHelper)
        {
            _mapper = mapper;
            _pathHelper = pathHelper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public BrandList(List<BrandDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<BrandList> Create(IQueryable<Brand> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new BrandList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Brand> source)
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

        public async Task<List<BrandDto>> GetDtos(IQueryable<Brand> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new BrandDto
                {
                    Id = c.Id,
                    Name= c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.BrandImagePath, c.ImageUrl) : "",
                    ProductMainCategoryId = c.ProductMainCategoryId,

                }).ToListAsync();
            return _mapper.Map<List<BrandDto>>(entities);
        }
    }
}