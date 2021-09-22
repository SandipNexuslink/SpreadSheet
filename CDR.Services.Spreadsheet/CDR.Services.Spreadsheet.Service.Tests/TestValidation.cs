using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Xunit;

namespace CDR.Services.Validation.Service.Tests
{
    public class TestValidation
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static HttpClient __httpClient;

        protected static HttpClient httpClient = __httpClient ??= new HttpClient();
        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);
        private readonly ILogger<IValidationService> logger = TestFactory.CreateLogger<IValidationService>();

        [Fact]
        public async void First_Valid_Validation_Request_WriteErrorsToBookFalse_Returns_Valid_RM_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new ValidationService(config, blobClient);
            var request = ValidationTestFactory.CreateFirstValidValidationRequestWithWriteErrorsToBookFalse();
            var result = await service.Validation(request);
            Assert.IsType<ValidationResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void InValid_Id_Validation_Request_Returns_Valid_Validation_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var request = ValidationTestFactory.CreateInvalidIdValidationRequest();
            var service = new ValidationService(config, blobClient);
            var result = await service.Validation(request);
            Assert.IsType<ValidationResponse>(result);
            Assert.NotNull(result);
        }
    }
}
