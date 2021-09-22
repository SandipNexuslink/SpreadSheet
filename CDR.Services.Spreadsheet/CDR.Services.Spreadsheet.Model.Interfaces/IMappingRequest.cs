using System;
using static CDR.Services.Spreadsheet.Model.Interfaces.SpreadSheet;

namespace CDR.Services.Spreadsheet.Model.Interfaces
{
    public interface IMappingRequest
    {
        public string Id { get; set; }
        public string BookToMap { get; set; }
        public string Template { get; set; }
        public MappingRequestOptions Options { get; set; }
        public DateTimeOffset Created { get; set; }
    }
    public class IMappingRequestOptions
    {
        SpreadsheetMapper Mapper { get; set; }
    }
    public class MappingRequestOptions : IMappingRequestOptions
    {
        public SpreadsheetMapper Mapper { get; set; }
    }

}
