using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CDR.Services.Spreadsheet.Service.Tests
{
    public class TestMapping
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        private static readonly ILogger<IMappingService> logger = MappingTestFactory.CreateLogger<MappingService>();

        [Fact]
        public async void First_Valid_MP_Request_WithoutBookToPersist_Returns_Valid_MP_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new MappingService(config, blobClient);
            var request = MappingTestFactory.CreateFirstValidMappingRequestWithoutBookToPersist();
            var result = await service.Mapping(request);
            Assert.IsType<MappingResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void Second_Valid_MP_Request_Returns_Valid_MP_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new MappingService(config, blobClient);
            var request = MappingTestFactory.CreateSecondValidMappingRequest();
            var result = await service.Mapping(request);
            Assert.IsType<MappingResponse>(result);
            Assert.NotNull(result);
        }
    }
}
