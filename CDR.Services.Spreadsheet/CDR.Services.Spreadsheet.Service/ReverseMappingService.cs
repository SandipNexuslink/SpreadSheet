using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service.Helper;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service
{
    public class ReverseMappingService : IReverseMappingService
    {
        private readonly IConfiguration _config;
        private readonly IAzureBlobStorageClient _blobClient;

        #region Private Variables
        private string _container;
        private SheetHelper _sheethelper;
        #endregion

        public ReverseMappingService(IConfiguration config, IAzureBlobStorageClient azureBlobStorageClient)
        {
            _config = config;
            _blobClient = azureBlobStorageClient;

            #region Setting Private Variables
            _container = _config["CDR:CDI:TemporaryFiles:Container"];
            _sheethelper = new SheetHelper(_config, _blobClient);
            #endregion
        }

        public async Task<IReverseMappingResponse> ReverseMapping(IReverseMappingRequest req)
        {
            // WHEN BookToPersist EXISTS, UPSERT BookToPersist BLOB ELSE UPSERT BookToMap AZURE BLOB
            var bookFilePath = !string.IsNullOrEmpty(req.BookToPersist) ? req.BookToPersist.Remove(0, _container.Length + 1) : req.BookToMap.Remove(0, _container.Length + 1);
            var templateFilePath = req.Template.Remove(0, _container.Length + 1);

            ExcelPackage excelPackage = await _sheethelper.GetSheet(bookFilePath);
            string jsonPayload = JsonConvert.SerializeObject(req.Payload);

            var renderedTemplate = await _sheethelper.GetRenderedTemplate(templateFilePath, jsonPayload);
            var jsonItems = JsonConvert.DeserializeObject<ExpandoObject>(renderedTemplate);

            foreach (var item in jsonItems)
            {
                var sheetAndCell = item.Key.Split('!');
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[sheetAndCell[0]]; // SELECTING SHEET
                worksheet.Cells[sheetAndCell[1]].Value = item.Value; // SETTING CELL VALUE
            }

            excelPackage.Workbook.Calculate(); // CALCULATE FORMULAS AFTER DATA UPDATE
            excelPackage.Save();

            await _sheethelper.UpsertSheet(bookFilePath, excelPackage); // SAVING SHEET IN AZURE

            return new ReverseMappingResponse
            {
                Id = req.Id,
                BookMapped = req.BookToMap,
                BookPersisted = req.BookToPersist,
                Created = DateTimeOffset.UtcNow
            };
        }
    }
}
