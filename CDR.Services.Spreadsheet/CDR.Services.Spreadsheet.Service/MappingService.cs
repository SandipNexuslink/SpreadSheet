using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service.Helper;
using CDR.Services.Spreadsheet.Service.Interfaces;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service
{
    public class MappingService : IMappingService
    {
        private readonly IConfiguration _config;
        private readonly IAzureBlobStorageClient _blobClient;
        //private static readonly CultureInfo culture = new CultureInfo("en-GB");


        public MappingService(IConfiguration config, IAzureBlobStorageClient azureBlobStorageClient)
        {
            _config = config;
            _blobClient = azureBlobStorageClient;
        }

        public async Task<IMappingResponse> Mapping(IMappingRequest req)
        {
            using (var helper = new SheetHelper(_config, _blobClient))
            {
                var Container = _config["CDR:CDI:TemporaryFiles:Container"];
                var response = new MappingResponse();
                /*Setting up blob storage starts*/

                response.BookToMap = req.BookToMap.Remove(0, Container.Length + 1);
                response.Template = req.Template.Remove(0, Container.Length + 1);
                var buvPackage = await helper.GetSheet(response.BookToMap);

                Stream stream = await helper.GetStream(response.BookToMap);
                var excelDataset = await ReadExcelAsync(stream);
                string json = JsonConvert.SerializeObject(excelDataset, Formatting.Indented);
                string templateContent = await helper.GetTemplateContent(response.Template);
                response.Payload = helper.RenderWithLiquidTemplate(templateContent, json);
                response.Id = Guid.NewGuid();
                response.Created = DateTimeOffset.Now;
                return response;
            }
        }


        private Task<DataSet> ReadExcelAsync(Stream stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:

                // 1. Use the reader methods
                do
                {
                    while (reader.Read())
                    {
                        // reader.GetDouble(0);
                    }
                } while (reader.NextResult());

                // 2. Use the AsDataSet extension method
                var result = reader.AsDataSet();

                return Task.FromResult(result);
                // The result of each spreadsheet is in result.Tables
            }
        }



    }
}
