using System;

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
    public class ValidationRequestOptions : IValidationRequestOptions
    {
        public bool WriteErrorsToBook { get; set; }
    }
    public interface IValidationRequestOptions
    {
        bool WriteErrorsToBook { get; set; }
    }
}
