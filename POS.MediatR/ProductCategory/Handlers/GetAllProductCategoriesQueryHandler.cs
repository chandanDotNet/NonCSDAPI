using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.IO;
using POS.Helper;

namespace POS.MediatR.Handler
{
    public class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, List<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllProductCategoriesQueryHandler(IProductCategoryRepository categoryRepository, IMapper mapper,
            POS.Helper.PathHelper pathHelper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }
        public async Task<List<ProductCategoryDto>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            //var categories = await _categoryRepository.All
            //    .Select(c => new ProductCategoryDto
            //    {
            //        Id = c.Id,
            //        Name = c.Name,
            //        ParentId = c.ParentId,
            //        Description = c.Description,
            //        ProductMainCategoryId = c.ProductMainCategoryId,
            //        ProductCategoryUrl = !string.IsNullOrWhiteSpace(c.ProductCategoryUrl) ?
            //        Path.Combine(_pathHelper.MasterCategoryImagePath, c.ProductCategoryUrl) : ""
            //    })
            //    .Where(c => request.IsDropDown || c.ParentId == request.Id)
            //    .OrderBy(c => c.Name)
            //    .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();

            var categories = await _categoryRepository.All
               .Select(c => new ProductCategoryDto
               {
                   Id = c.Id,
                   Name = c.Name,
                   ParentId = c.ParentId,
                   Description = c.Description,
                   ProductMainCategoryId = c.ProductMainCategoryId,
                   ProductCategoryUrl = !string.IsNullOrWhiteSpace(c.ProductCategoryUrl) ?
                   Path.Combine(_pathHelper.ProductCategoryImagePath, c.ProductCategoryUrl) : "",
                   ProductMainCategoryName = c.ProductMainCategory.Name
               })
               .Where(c => request.IsDropDown || c.ParentId == request.Id)
               .OrderBy(c => c.Name)
               .ToListAsync();

            return categories;
        }
    }
}
