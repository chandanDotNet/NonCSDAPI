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
    public class UserSupplierRepository : GenericRepository<UserSupplier, POSDbContext>, IUserSupplierRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public UserSupplierRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
          : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }
        public async Task<UserSupplierList> GetUserSuppliers(UserSupplierResource userSupplierResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(userSupplierResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<UserSupplierDto, UserSupplier>());           

            if (userSupplierResource.UserId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.UserId == userSupplierResource.UserId.Value);
            }

            var UserSupplierList = new UserSupplierList(_mapper);
            return await UserSupplierList.Create(collectionBeforePaging, userSupplierResource.Skip, userSupplierResource.PageSize);
        }
    }
}
