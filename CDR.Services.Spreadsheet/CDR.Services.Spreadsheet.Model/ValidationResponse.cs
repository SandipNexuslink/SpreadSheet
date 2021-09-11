using CDR.Services.Spreadsheet.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDR.Services.Spreadsheet.Model
{
    public class ValidationResponse : IValidationResponse
    {
        public DateTimeOffset? DateCalculated { get; set; }
    }
}
