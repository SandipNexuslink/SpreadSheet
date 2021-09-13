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
using System;
using System.IO;
using System.Threading.Tasks;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "validation-request")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var request = await req.GetJsonBody<ValidationRequest, ValidationRequestValidator>();
                if (!request.IsValid)
                {
                    log.LogInformation($"Invalid request.");
                    return request.ToBadRequest();
                }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                IValidationRequest data = JsonConvert.DeserializeObject<ValidationRequest>(requestBody);
                IValidationResponse responseMessage = await _validationService.Validation(data);
                return new OkObjectResult(responseMessage);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
