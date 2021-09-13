using CDR.Services.Spreadsheet.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CDR.Services.Spreadsheet.Model
{
    public class ValidationResponse : IValidationResponse
    {
        public ValidationResponse()
        {
            Results = new List<ValidationResult>();
        }
        public string id { get; set; }
        public string BookUnderValidation { get; set; }
        public string ValidatorBook { get; set; }
        public DateTimeOffset? Created { get; set; }
        public bool IsValid { get; set; }
        public List<ValidationResult> Results { get; set; }
    }
}
