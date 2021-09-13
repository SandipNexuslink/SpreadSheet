using CDR.Services.Spreadsheet.CommonTestAssets;
using CDR.Services.Spreadsheet.Service.Interfaces;
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

        private readonly ILogger<IValidationService> logger = TestFactory.CreateLogger<IValidationService>();

        [Fact]
        public async void Valid_RC_Request_Returns_Valid_RC_Response()
        {
            //await TestConfiguration.RefreshConfiguration();
            //var service = new ValidationService(config, logger, httpClient);
            //var request = TestFactory.CreateValidValidationRequest();
            //var result = await service.Calculate(request);
            //Assert.IsType<ValidationResponse>(result);
            //Assert.NotNull(result);
        }


    }
}
