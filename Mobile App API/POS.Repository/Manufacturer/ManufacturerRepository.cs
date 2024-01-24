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
    public class ManufacturerRepository
        : GenericRepository<Manufacturer, POSDbContext>, IManufacturerRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public ManufacturerRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
          : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<ManufacturerList> GetManufacturers(ManufacturerResource manufacturerResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(manufacturerResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ManufacturerDto, Manufacturer>());

            if (!string.IsNullOrEmpty(manufacturerResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = manufacturerResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.ManufacturerName, $"{encodingName}%"));
            }
            var ManufacturerList = new ManufacturerList(_mapper);
            return await ManufacturerList.Create(collectionBeforePaging, manufacturerResource.Skip, manufacturerResource.PageSize);
        }
    }
}
