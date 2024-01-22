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
        private readonly PathHelper _pathHelper;
        public BrandRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService,
             IMapper mapper, PathHelper pathHelper)
          : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
            _pathHelper = pathHelper;
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

            if (brandResource.ProductMainCategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProductMainCategoryId == brandResource.ProductMainCategoryId.Value);
            }

            var BrandList = new BrandList(_mapper,_pathHelper);
            return await BrandList.Create(collectionBeforePaging, brandResource.Skip, brandResource.PageSize);
        }
    }
}
