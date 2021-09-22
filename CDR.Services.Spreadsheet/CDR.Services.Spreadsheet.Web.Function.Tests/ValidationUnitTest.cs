using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace CDR.Services.Spreadsheet.Web.Function.Tests
{
    public class ValidationUnitTest
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        private static readonly ILogger<IValidationService> logger = ValidationTestFactory.CreateLogger<IValidationService>();

        #region Validation

        private static async Task<IActionResult> _runValidationTest(IValidationRequest request)
        {
            await TestConfiguration.RefreshConfiguration();
            var httpRequest = FunctionTestFactory.CreateHttpPostRequest(request);
            var service = new ValidationService(config, blobClient);
            var function = new Validation(service);
            var httpResponse = await function.Run(httpRequest, logger);
            return httpResponse;
        }

        [Fact]
        public async void Invalid_Validation_Request_Returns_BadRequest()
        {
            var request = ValidationTestFactory.CreateInvalidEntireValidationRequest();
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Empty_Validation_Request_Returns_BadRequest()
        {
            var request = ValidationTestFactory.CreateEmptyValidationRequest();
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_NullBookUnder_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = ValidationTestFactory.CreateInvalidBookUnderValidationRequest(true);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_Id_Validation_Request_Returns_InValid_Validation_Response()
        {
            var request = ValidationTestFactory.CreateInvalidIdValidationRequest();
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Valid_Validation_Request_Returns_Valid_Validation_Response()
        {
            var request = ValidationTestFactory.CreateValidValidationRequest();
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }

        [Fact]
        public async void InValid_EmptyBookUnder_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = ValidationTestFactory.CreateInvalidBookUnderValidationRequest(false);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_EmptyValidatorBook_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = ValidationTestFactory.CreateValidatorBookValidationRequest(false);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void InValid_NullValidatorBook_Validation__Request_Returns_InValid_RM_Response()
        {
            var request = ValidationTestFactory.CreateValidatorBookValidationRequest(true);
            var response = (BadRequestObjectResult)await _runValidationTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Valid_Validation_Request_Returns_TRUE_WriteErrorsToBook_Value()
        {
            var request = ValidationTestFactory.CreateWriteErrorsToBookValidationRequest(true);
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }
        [Fact]
        public async void Valid_Validation_Request_Returns_FALSE_WriteErrorsToBook_Value()
        {
            var request = ValidationTestFactory.CreateWriteErrorsToBookValidationRequest(false);
            var response = (OkObjectResult)await _runValidationTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IValidationResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IValidationResponse>(result);
        }
        #endregion
    }
}
