using AutoMapper;
using ExcelDataReader;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.ExcelUpload.Command;
using POS.MediatR.Handlers;
using POS.MediatR.Inventory.Command;

using POS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.ExcelUpload.Handlers
{
    public class AddStockExcelUploadCommandHandler : IRequestHandler<AddStockExcelUploadCommand, ServiceResponse<StockExcelUploadDto>>
    {
       
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddStockExcelUploadCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IExcelDataReader reader;
        public AddStockExcelUploadCommandHandler(
           
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddStockExcelUploadCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
           
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<StockExcelUploadDto>> Handle(AddStockExcelUploadCommand request, CancellationToken cancellationToken)
        {
            StockExcelUploadDto entityToReturn = new StockExcelUploadDto();
            if (request.FileDetails != null)
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.StockExcelUploadFilePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await using(FileStream stream = new FileStream(Path.Combine(pathToSave,
                    request.FileDetails.FileName), FileMode.Create))
                {
                    request.FileDetails.CopyTo(stream);
                }

                //=============CM Code
                //string dataFileName = Path.GetFileName(request.FileDetails.FileName);

                //string extension = Path.GetExtension(dataFileName);

                //string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };
                //// USe this to handle Encodeing differences in .NET Core
                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //// read the excel file
                //using (var stream = new FileStream(saveToPath, FileMode.Open))
                //{
                //    if (extension == ".xls")
                //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                //    else
                //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                //    //var conf = new ExcelDataSetConfiguration
                //    //{
                //    //    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                //    //    {
                //    //        UseHeaderRow = true
                //    //    }
                //    //};

                //    DataSet ds = new DataSet();
                //    ds = reader.AsDataSet();
                //    reader.Close();

                //    if (ds != null && ds.Tables.Count > 0)
                //    {
                //        // Read the the Table
                //        DataTable serviceDetails = ds.Tables[0];
                //        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                //        {
                //            Guid ProId;

                //            AddInventoryCommand details = new AddInventoryCommand();
                //            details.ProductId = new Guid(serviceDetails.Rows[i][0].ToString());
                //            details.Stock = Convert.ToInt64(serviceDetails.Rows[i][1].ToString());
                //            details.PricePerUnit = Convert.ToDecimal(serviceDetails.Rows[i][2].ToString());
                //            details.UnitId = new Guid(serviceDetails.Rows[i][3].ToString());

                //            // Add the record in Database
                //            //await _mediator.Send(addInventoryCommand);
                //            //await context.SaveChangesAsync();
                //        }
                //    }
                //}

                //=========================

                //await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), item.ImageUrlData);
            }
            return ServiceResponse<StockExcelUploadDto>.ReturnResultWith200(entityToReturn);
        }
    }
}