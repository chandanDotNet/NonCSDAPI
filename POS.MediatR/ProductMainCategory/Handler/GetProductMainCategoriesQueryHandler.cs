using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;

namespace POS.MediatR.Handler
{
    public class GetProductMainCategoriesQueryHandler : IRequestHandler<GetProductMainCategoriesQuery, List<ProductMainCategoryDto>>
    {
        private readonly IProductMainCategoryRepository _mainCategoryRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetProductMainCategoriesQueryHandler(IProductMainCategoryRepository mainCategoryRepository, IMapper mapper,
             PathHelper pathHelper)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }
        public async Task<List<ProductMainCategoryDto>> Handle(GetProductMainCategoriesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _mainCategoryRepository.All
               .Select(c => new ProductMainCategoryDto
               {
                   Id = c.Id,
                   Name = c.Name,
                   CategoryImageUrl = !string.IsNullOrWhiteSpace(c.CategoryImageUrl) ?
                   Path.Combine(_pathHelper.MasterCategoryImagePath, c.CategoryImageUrl) : "",
                   Description = c.Description
               }).ToListAsync();

            return entities;

            //var categories = await _mainCategoryRepository.All                
            //    .OrderBy(c => c.Name)
            //    .ProjectTo<ProductMainCategoryDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            //return categories;
        }
    }
}