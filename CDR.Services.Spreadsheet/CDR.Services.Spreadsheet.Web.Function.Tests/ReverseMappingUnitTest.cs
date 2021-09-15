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
    public class ReverseMappingUnitTest
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        private static readonly ILogger<IReverseMappingService> logger = ReverseMappingTestFactory.CreateLogger<IReverseMappingService>();

       
        private static async Task<IActionResult> _runReserveMappingTest(IReverseMappingRequest request)
        {
            await TestConfiguration.RefreshConfiguration();
            var httpRequest = FunctionTestFactory.CreateHttpPostRequest(request);
            var service = new ReverseMappingService(config, blobClient);
            var function = new ReverseMapping(service);
            var httpResponse = await function.Run(httpRequest, logger);
            return httpResponse;
        }

        [Fact]
        public async void Valid_RM_First_Request_WithoutBookToPersist_Returns_Valid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateFirstValidReverseMappingRequestWithoutBookToPersist();
            var response = (OkObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IReverseMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IReverseMappingResponse>(result);
        }

        [Fact]
        public async void Valid_RM_Second_Request_WithoutBookToPersist_Returns_Valid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateSecondValidReverseMappingRequestWithoutBookToPersist();
            var response = (OkObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IReverseMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IReverseMappingResponse>(result);
        }

        [Fact]
        public async void Valid_RM_First_Request_WithBookToPersist_Returns_Valid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateFirstValidReverseMappingRequestWithBookToPersist();
            var response = (OkObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IReverseMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IReverseMappingResponse>(result);
        }

        [Fact]
        public async void Valid_RM_Second_Request_WithBookToPersist_Returns_Valid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateSecondValidReverseMappingRequestWithBookToPersist();
            var response = (OkObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IReverseMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IReverseMappingResponse>(result);
        }


        [Fact]
        public async void Invalid_EmptyRequest_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateEmptyReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Id_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateInvalidIdReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_BookToMap_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateInvalidBookToMapReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Payload_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateInvalidPayloadReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Template_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateInvalidTemplateReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_EntireRequest_RM_Request_Returns_Invalid_RM_Response()
        {
            var request = ReverseMappingTestFactory.CreateInvalidEntireReverseMappingRequest();
            var response = (BadRequestObjectResult)await _runReserveMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }
    }
}
