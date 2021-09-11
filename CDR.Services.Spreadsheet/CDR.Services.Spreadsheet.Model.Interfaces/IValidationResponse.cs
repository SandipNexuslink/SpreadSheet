using System;
using System.ComponentModel.DataAnnotations;

namespace CDR.Services.Spreadsheet.Model.Interfaces
{
    public interface IValidationResponse
    {
        [Display(Name = "Date Calculated")]
        DateTimeOffset? DateCalculated { get; set; }
    }
}
