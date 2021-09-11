using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service.Helper;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service
{
    public class ValidationService : IValidationService
    {
        private readonly IConfiguration _config;

        //private static readonly CultureInfo culture = new CultureInfo("en-GB");


        public ValidationService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
        }

        public async Task<IValidationResponse> Validation(IValidationRequest req)
        {
            try
            {
                var response = new ValidationResponse();
                SheetHelper helper = new SheetHelper();

                /*Setting up blob storage starts*/

                string storageAccount_connectionString = _config["Azure:CloudStorage:ConnectionString"];
                var temporaryContainer = _config["CDR:CDI:TemporaryFiles:Container"];
                var buvFilePath = req.BookUnderValidation.Remove(0, temporaryContainer.Length + 1);
                var vbFilePath = req.ValidatorBook.Remove(0, temporaryContainer.Length + 1);
                CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
                CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

                /*Setting up blob storage end*/

                var buvPackage = helper.GetSheet(blobClient, temporaryContainer, buvFilePath).Result;

                var vdPackage = helper.GetSheet(blobClient, temporaryContainer, vbFilePath).Result;

                var masterPackage = helper.MergeSheets(buvPackage, vdPackage);

                masterPackage = helper.ClearCommentsAndStyle(masterPackage);

                response.DateCalculated = DateTimeOffset.Now;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
