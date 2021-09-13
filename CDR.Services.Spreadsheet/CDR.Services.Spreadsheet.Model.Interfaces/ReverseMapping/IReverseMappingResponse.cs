using System;

namespace CDR.Services.Spreadsheet.Model.Interfaces
{
    public interface IReverseMappingResponse
    {
        public Guid Id { get; set; }
        public string BookMapped { get; set; }
        public string BookPersisted { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
