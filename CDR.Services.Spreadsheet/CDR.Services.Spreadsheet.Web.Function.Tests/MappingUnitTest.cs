using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CDR.Services.Spreadsheet.Web.Function.Tests
{
    public class MappingUnitTest
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        private static readonly ILogger<IMappingService> logger = MappingTestFactory.CreateLogger<IMappingService>();

        private static async Task<IActionResult> _runMappingTest(IMappingRequest request)
        {
            await TestConfiguration.RefreshConfiguration();
            var httpRequest = FunctionTestFactory.CreateHttpPostRequest(request);
            var service = new MappingService(config, blobClient);
            var function = new Mapping(service);
            var httpResponse = await function.Run(httpRequest, logger);
            return httpResponse;
        }

        [Fact]
        public async void Valid_MP_First_Request_WithOptions_Returns_Valid_MP_Response()
        {
            var request = MappingTestFactory.CreateFirstValidMappingRequestWithOptions();
            var response = (OkObjectResult)await _runMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IMappingResponse>(result);
        }

        [Fact]
        public async void Valid_MP_First_Request_Returns_Valid_MP_Response()
        {
            var request = MappingTestFactory.CreateSecondValidMappingRequest();
            var response = (OkObjectResult)await _runMappingTest(request);
            Assert.Equal(200, response.StatusCode);
            var result = response.Value as IMappingResponse;
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IMappingResponse>(result);
        }

        [Fact]
        public async void Invalid_EmptyRequest_MP_Request_Returns_Invalid_MP_Response()
        {
            var request = MappingTestFactory.CreateEmptyMappingRequest();
            var response = (BadRequestObjectResult)await _runMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Id_MP_Request_Returns_Invalid_MP_Response()
        {
            var request = MappingTestFactory.CreateInvalidIdMappingRequest();
            var response = (BadRequestObjectResult)await _runMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_BookToMap_MP_Request_Returns_Invalid_MP_Response()
        {
            var request = MappingTestFactory.CreateInvalidBookToMapMappingRequest();
            var response = (BadRequestObjectResult)await _runMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_Template_MP_Request_Returns_Invalid_MP_Response()
        {
            var request = MappingTestFactory.CreateInvalidTemplateMappingRequest();
            var response = (BadRequestObjectResult)await _runMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async void Invalid_EntireRequest_MP_Request_Returns_Invalid_MP_Response()
        {
            var request = MappingTestFactory.CreateInvalidEntireMappingRequest();
            var response = (BadRequestObjectResult)await _runMappingTest(request);
            Assert.Equal(400, response.StatusCode);
        }
    }
}
