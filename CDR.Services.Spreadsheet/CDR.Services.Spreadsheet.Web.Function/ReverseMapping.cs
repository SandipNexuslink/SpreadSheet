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
    public class ReverseMapping
    {
        private readonly IReverseMappingService _reverseMappingService;

        public ReverseMapping(IReverseMappingService reverseMappingService)
        {
            _reverseMappingService = reverseMappingService;
        }

        [FunctionName("ReverseMapping")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reverse-mapping-request")] HttpRequest req,
            ILogger log)
        {
            // Empty body >> req.ContentLength == 0 from Request || req.Body.Length == 4 from Test cases
            if (req.ContentLength == 0 || req.Body.Length == 4)
            {
                log.LogInformation("Invalid request >> Empty Body");
                return new BadRequestObjectResult("Bad Request");
            }

            var request = await req.GetJsonBody<ReverseMappingRequest, ReverseMappingRequestValidator>();
            if (!request.IsValid)
            {
                log.LogInformation($"Invalid request >> Validations >> Request: {request}");
                return request.ToBadRequest();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IReverseMappingRequest data = JsonConvert.DeserializeObject<ReverseMappingRequest>(requestBody);
            IReverseMappingResponse responseMessage = await _reverseMappingService.ReverseMapping(data);

            return new OkObjectResult(responseMessage);
        }
    }
}
