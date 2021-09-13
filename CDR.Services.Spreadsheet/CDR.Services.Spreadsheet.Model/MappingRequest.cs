using CDR.Services.Spreadsheet.Model.Interfaces;
using System;

namespace CDR.Services.Spreadsheet.Model
{
    public class MappingRequest : IMappingRequest
    {
        public DateTimeOffset Created { get; set; }
        public string Id { get; set; }
        public string BookToMap { get; set; }
        public string Template { get; set; }
        public IMappingRequestOptions Options { get; set; }
    }
}
