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
    public class ProductTypeRepository : GenericRepository<ProductType, POSDbContext>, IProductTypeRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public ProductTypeRepository(IUnitOfWork<POSDbContext> uow, IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<ProductTypeList> GetProductTypes(ProductTypeResource productTypeResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(productTypeResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ManufacturerDto, Manufacturer>());

            if (!string.IsNullOrEmpty(productTypeResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = productTypeResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%"));
            }
            var ProductTypeList = new ProductTypeList(_mapper);
            return await ProductTypeList.Create(collectionBeforePaging, productTypeResource.Skip, productTypeResource.PageSize);
        }
    }
}
