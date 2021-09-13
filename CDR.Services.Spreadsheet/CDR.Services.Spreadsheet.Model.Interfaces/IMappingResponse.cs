using System;

namespace CDR.Services.Spreadsheet.Model.Interfaces
{
    public interface IMappingResponse
    {
        public Guid Id { get; set; }
        public dynamic Payload { get; set; }
        public string BookToMap { get; set; }
        public string Template { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
