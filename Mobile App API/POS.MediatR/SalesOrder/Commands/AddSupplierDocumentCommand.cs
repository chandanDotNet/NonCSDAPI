using MediatR;
using Microsoft.AspNetCore.Components.Web;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Supplier.Commands
{
    public class AddSupplierDocumentCommand : IRequest<ServiceResponse<bool>>
    {
        public List<DocumentList> SupplierDocuments { get; set; }
    }
    public class DocumentList
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public string DocumentData { get; set; }
        public string FileExtension { get; set; }
    }
}
