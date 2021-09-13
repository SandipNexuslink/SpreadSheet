using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CDR.Services.Spreadsheet.Service.Interfaces;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using CDR.Services.Spreadsheet.Web.Function.Validator;

namespace CDR.Services.Spreadsheet.Web.Function
{
    public class Validation
    {
        private readonly IValidationService _validationService;

        public Validation(IValidationService validationService)
        {
            _validationService = validationService;
        }

        [FunctionName("Validation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var request = await req.GetJsonBody<ValidationRequest, ValidationRequestValidator>();
            if (!request.IsValid)
            {
                log.LogInformation($"Invalid request.");
                return request.ToBadRequest();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IValidationRequest data = JsonConvert.DeserializeObject<ValidationRequest>(requestBody);
            IValidationResponse responseMessage=await _validationService.Validation(data);
            return new OkObjectResult(responseMessage);
        }
    }
}
