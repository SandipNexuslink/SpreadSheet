using Azure.Storage.Blobs.Models;
using CDR.Azure.Storage.Blob.Client;
using DotLiquid;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service.Helper
{
    public class SheetHelper : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IAzureBlobStorageClient _blobClient;
        public SheetHelper(IConfiguration config, IAzureBlobStorageClient azureBlobStorageClient)
        {
            _config = config;
            _blobClient = azureBlobStorageClient;
        }
        private string Container { get { return _config["CDR:CDI:TemporaryFiles:Container"]; } }
        private string ValidatorBookName { get { return _config["ExcelProperties:ValidatorBookName"]; } }

        public async Task<ExcelPackage> GetSheet(string FilePath)
        {
            try
            {
                Stream stream = await _blobClient.Get(Container, FilePath);
                ExcelPackage Package = new ExcelPackage(stream);
                return Package;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Stream> GetStream(string FilePath)
        {
            return await _blobClient.Get(Container, FilePath);
        }

        public async Task<string> GetTemplateContent(string templateFilePath)
        {
            using (Stream templateStream = await GetStream(templateFilePath))
            {
                var streamReader = new StreamReader(templateStream);
                return await streamReader.ReadToEndAsync();
            }
        }

        public async Task<string> GetRenderedTemplate(string templateFilePath, string jsonPayload)
        {
            string templateContent = await GetTemplateContent(templateFilePath);
            Template template = Template.Parse(templateContent);

            dynamic expandoObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());
            IDictionary<string, object> expandoDict = new Dictionary<string, object>(expandoObj);
            return template.Render(Hash.FromDictionary(expandoDict));
        }

        public async Task UpsertSheet(string FilePath, ExcelPackage package)
        {
            BlobProperties blobProperties = await GetBlobProperties(FilePath); // FOR COLLECTING MIME TYPE OF AZURE BLOB
            package.Stream.Position = 0; // SETTING UP CONTENT POSITION TO 0
            await _blobClient.Upsert(Container, FilePath, package.Stream, blobProperties.ContentType);
        }

        public async Task<BlobProperties> GetBlobProperties(string FilePath)
        {
            return await _blobClient.GetBlobProperties(Container, FilePath);
        }

        //SerializeObject pass in parameter
        public string RenderWithLiquidTemplate(string templateContent, string jsonObj)
        {
            Template template = Template.Parse(templateContent);
            dynamic expandoObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonObj, new ExpandoObjectConverter());
            IDictionary<string, object> expandoDict = new Dictionary<string, object>(expandoObj);
            return template.Render(Hash.FromDictionary(expandoDict));
        }
        public void Dispose()
        {
            this.Dispose();
        }
    }
}
