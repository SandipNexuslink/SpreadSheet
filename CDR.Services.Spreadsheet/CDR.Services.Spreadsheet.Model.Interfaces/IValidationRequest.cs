using System;
using System.Collections.Generic;
using System.Text;

namespace CDR.Services.Spreadsheet.Model.Interfaces
{
    public interface IValidationRequest
    {
        public string Id { get; set; }
        public string BookUnderValidation { get; set; }
        public string ValidatorBook { get; set; }
        public ValidationRequestOptions Options { get; set; }
        public DateTimeOffset Created { get; set; }
    }
    public class ValidationRequestOptions
    {
        public bool WriteErrorsToBook { get; set; }
    }
}
