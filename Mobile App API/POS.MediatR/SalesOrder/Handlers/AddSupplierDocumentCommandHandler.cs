using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Supplier.Commands;
using POS.MediatR.Banner.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Supplier.Handlers
{
    public class AddSupplierDocumentCommandHandler : IRequestHandler<AddSupplierDocumentCommand, ServiceResponse<bool>>
    {
        private readonly ISupplierDocumentRepository _supplierDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddSupplierDocumentCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddSupplierDocumentCommandHandler(
           ISupplierDocumentRepository supplierDocumentRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddSupplierDocumentCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _supplierDocumentRepository = supplierDocumentRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<bool>> Handle(AddSupplierDocumentCommand request, CancellationToken cancellationToken)
        {
            //var existingEntity = await _supplierDocumentRepository.FindBy(x=>x.SupplierId == request.SupplierDocuments[0].SupplierId)
            //.ToListAsync();

            //if (existingEntity != null)
            //{
            //    _logger.LogError("Supplier Document Already Exist");
            //    return ServiceResponse<SupplierDocumentDto>.Return409("Supplier Document Already Exist.");
            //}
            foreach (var item in request.SupplierDocuments)
            {
                var supplierDocs = new SupplierDocument()
                {
                    Name = item.Name,
                    SupplierId = item.SupplierId,
                    Documents = Guid.NewGuid().ToString() + item.FileExtension,
                };
                var entity = _mapper.Map<Data.SupplierDocument>(supplierDocs);
                _supplierDocumentRepository.Add(entity);

                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Save Page have Error");
                    return ServiceResponse<bool>.Return500();
                }

                if (!string.IsNullOrWhiteSpace(item.DocumentData))
                {
                    var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.SupplierDocumentImagePath);
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    await FileData.SaveFile(Path.Combine(pathToSave, entity.Documents), item.DocumentData);
                }
                var entityToReturn = _mapper.Map<SupplierDocumentDto>(entity);
                if (!string.IsNullOrWhiteSpace(item.DocumentData))
                {
                    entityToReturn.Documents = Path.Combine(_pathHelper.SupplierDocumentImagePath, entityToReturn.Documents);
                }
            }

            //if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            //{
            //    entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            //}            


            //if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            //{
            //    var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.BannerImagePath);
            //    if (!Directory.Exists(pathToSave))
            //    {
            //        Directory.CreateDirectory(pathToSave);
            //    }
            //    await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            //}
            //var entityToReturn = _mapper.Map<SupplierDocumentDto>(entity);
            //if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            //{
            //    entityToReturn.ImageUrl = Path.Combine(_pathHelper.BannerImagePath, entityToReturn.ImageUrl);
            //}
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}