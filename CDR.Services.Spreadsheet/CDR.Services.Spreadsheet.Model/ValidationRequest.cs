using CDR.Services.Spreadsheet.Model.Interfaces;
using System;

namespace CDR.Services.Spreadsheet.Model
{
    public class ValidationRequest : IValidationRequest
    {
        public string Id { get; set; }
        public string BookUnderValidation { get; set; }
        public string ValidatorBook { get; set; }
        public ValidationRequestOptions Options { get; set; }
        public DateTimeOffset Created { get; set; }        
    }

}
