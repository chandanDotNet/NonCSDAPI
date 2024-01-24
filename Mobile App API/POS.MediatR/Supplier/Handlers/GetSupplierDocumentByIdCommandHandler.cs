using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Handlers;
using POS.MediatR.Supplier.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Supplier.Handlers
{
    public class GetSupplierDocumentByIdCommandHandler : IRequestHandler<GetSupplierDocumentByIdCommand, ServiceResponse<List<SupplierDocumentDto>>>
    {
        private readonly ISupplierDocumentRepository _supplierDocumentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSupplierDocumentByIdCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetSupplierDocumentByIdCommandHandler(
           ISupplierDocumentRepository supplierDocumentRepository,
            IMapper mapper,
            ILogger<GetSupplierDocumentByIdCommandHandler> logger,
             PathHelper pathHelper
            )
        {

            _mapper = mapper;
            _supplierDocumentRepository = supplierDocumentRepository;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<List<SupplierDocumentDto>>> Handle(GetSupplierDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = await _supplierDocumentRepository.AllIncluding()
                .Where(s => s.SupplierId == request.Id).ToListAsync();
            if (entity != null)
            {
                var entityDto = _mapper.Map<List<SupplierDocumentDto>>(entity);

                var entities = entity
               .Select(c => new SupplierDocumentDto
               {
                   Id = c.Id,
                   Name = c.Name,
                   SupplierId= c.SupplierId,
                   Documents = !string.IsNullOrWhiteSpace(c.Documents) ? Path.Combine(_pathHelper.SupplierDocumentImagePath, c.Documents) : ""
               }).ToList();
                //return entities;
                //entityDto.Documents = string.IsNullOrWhiteSpace(entityDto.Documents) ? ""
                //    : Path.Combine(_pathHelper.SupplierDocumentImagePath, entityDto.Documents);
                return ServiceResponse<List<SupplierDocumentDto>>.ReturnResultWith200(entities);

            }
            else
            {
                _logger.LogError("User not found");
                return ServiceResponse<List<SupplierDocumentDto>>.ReturnFailed(404, "User not found");
            }
        }
    }
}