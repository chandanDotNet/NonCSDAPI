using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class BrandRepository 
        : GenericRepository<Brand, POSDbContext>, IBrandRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public BrandRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService,
             IMapper mapper)
          : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BrandList> GetBrands(BrandResource brandResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(brandResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<BrandDto, Brand>());

            if (!string.IsNullOrEmpty(brandResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = brandResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%"));
            }            
            var BrandList = new BrandList(_mapper);
            return await BrandList.Create(collectionBeforePaging, brandResource.Skip, brandResource.PageSize);
        }
    }
}
