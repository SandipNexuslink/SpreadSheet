using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CDR.Services.Spreadsheet.Web.Function.Tests
{
    public class UnitTest1
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static HttpClient __httpClient;

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        protected static HttpClient httpClient = __httpClient ??= new HttpClient();

        private static readonly ILogger<IValidationService> logger = TestFactory.CreateLogger<IValidationService>();
        private ValidationResult _validationResult;

        #region Validation

        private static async Task<IActionResult> _runValidationTest(IValidationRequest request)
        {
            await TestConfiguration.RefreshConfiguration();
            var httpRequest = FunctionTestFactory.CreateHttpPostRequest(request);
            var service = new ValidationService(config, httpClient);
            var function = new Validation(service);
            var httpResponse = await function.Run(httpRequest, logger);
            return httpResponse;
        }

        [Fact]
        public async void Invalid_Validation_Request_Returns_BadRequest()
        {
            var request = TestFactory.CreateInvalidValidationRequest();
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_NullBookUnder_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = TestFactory.CreateSecondValidationRequest(true);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Valid_Validation_Request_Returns_Valid_Validation_Response()
        {
            var request = TestFactory.CreateValidValidationRequest();
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }

        [Fact]
        public async void InValid_EmptyBookUnder_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = TestFactory.CreateSecondValidationRequest(false);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_EmptyValidatorBook_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = TestFactory.CreateValidatorBookValidationRequest(false);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_NullValidatorBook_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = TestFactory.CreateValidatorBookValidationRequest(true);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Valid_Validation_Request_Returns_TRUE_WriteErrorsToBook_Value()
        {
            var request = TestFactory.CreateWriteErrorsToBookValidationRequest(true);
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }
        [Fact]
        public async void Valid_Validation_Request_Returns_FALSE_WriteErrorsToBook_Value()
        {
            var request = TestFactory.CreateWriteErrorsToBookValidationRequest(false);
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }
        #endregion
    }
}
