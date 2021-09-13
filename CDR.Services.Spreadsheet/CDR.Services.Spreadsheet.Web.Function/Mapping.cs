using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service.Interfaces;
using CDR.Services.Spreadsheet.Web.Function.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
namespace CDR.Services.Spreadsheet.Web.Function
{
    public class Mapping
    {

        private readonly IMappingService _mappingService;
        public Mapping(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }
        [FunctionName("Mapping")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mapping-request")] HttpRequest req,
            ILogger log)
        {
            var request = await req.GetJsonBody<MappingRequest, MappingRequestValidator>();
            if (!request.IsValid)
            {
                log.LogInformation($"Invalid request.");
                return request.ToBadRequest();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IMappingRequest data = JsonConvert.DeserializeObject<MappingRequest>(requestBody);
            var responseMessage = await _mappingService.Mapping(data);
            return new OkObjectResult(responseMessage);
        }
    }
}
