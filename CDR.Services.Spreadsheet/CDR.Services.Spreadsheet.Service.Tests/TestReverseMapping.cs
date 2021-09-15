using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CDR.Services.Spreadsheet.Service.Tests
{
    public class TestReverseMapping
    {
        private static IConfiguration __config;

        protected static IConfiguration config = __config ??= TestConfiguration.GetConfiguration();

        private static IAzureBlobStorageClient _blobClient;

        protected static IAzureBlobStorageClient blobClient = _blobClient ??= new AzureBlobStorageClient(config);

        private static readonly ILogger<IReverseMappingService> logger = ReverseMappingTestFactory.CreateLogger<IReverseMappingService>();

        [Fact]
        public async void First_Valid_RM_Request_WithoutBookToPersist_Returns_Valid_RM_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new ReverseMappingService(config, blobClient);
            var request = ReverseMappingTestFactory.CreateFirstValidReverseMappingRequestWithoutBookToPersist();
            var result = await service.ReverseMapping(request);
            Assert.IsType<ReverseMappingResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void Second_Valid_RM_Request_WithoutBookToPersist_Returns_Valid_RM_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new ReverseMappingService(config, blobClient);
            var request = ReverseMappingTestFactory.CreateSecondValidReverseMappingRequestWithoutBookToPersist();
            var result = await service.ReverseMapping(request);
            Assert.IsType<ReverseMappingResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void Third_Valid_RM_Request_WithBookToPersist_Returns_Valid_RM_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new ReverseMappingService(config, blobClient);
            var request = ReverseMappingTestFactory.CreateFirstValidReverseMappingRequestWithBookToPersist();
            var result = await service.ReverseMapping(request);
            Assert.IsType<ReverseMappingResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async void Fourth_Valid_RM_Request_WithBookToPersist_Returns_Valid_RM_Response()
        {
            await TestConfiguration.RefreshConfiguration();
            var service = new ReverseMappingService(config, blobClient);
            var request = ReverseMappingTestFactory.CreateSecondValidReverseMappingRequestWithBookToPersist();
            var result = await service.ReverseMapping(request);
            Assert.IsType<ReverseMappingResponse>(result);
            Assert.NotNull(result);
        }
    }
}
