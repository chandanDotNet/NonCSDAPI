using AutoMapper;
using MediatR;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.ShopHoliday.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POS.MediatR.ShopHoliday.Handler
{
    public class GetShopHolidayCommandHandler : IRequestHandler<GetShopHolidayCommand, ServiceResponse<ShopHolidayDto>>
    {
        private readonly IShopHolidayRepository _shopHolidayRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetShopHolidayCommandHandler> _logger;
        public GetShopHolidayCommandHandler(
           IShopHolidayRepository shopHolidayRepository,
            IMapper mapper,
            ILogger<GetShopHolidayCommandHandler> logger
            )
        {
            _shopHolidayRepository = shopHolidayRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<ShopHolidayDto>> Handle(GetShopHolidayCommand request, CancellationToken cancellationToken)
        {
            var objEntityDto = await _shopHolidayRepository.All           
            .ProjectTo<ShopHolidayDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            var _list = from t1 in objEntityDto
                           where t1.FromDate <= DateTime.Now
                           where DateTime.Now <= t1.ToDate
                           select t1;

            var entityDto = _list.FirstOrDefault();

            if (entityDto == null)
            {
                //_logger.LogError("Holiday is not exists");
                return ServiceResponse<ShopHolidayDto>.Return404("No Data Found");
            }
            return ServiceResponse<ShopHolidayDto>.ReturnResultWith200(entityDto);
        }
    }
}