using CDR.Services.Spreadsheet.Model.Interfaces;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service.Interfaces
{
    public interface IMappingService
    {
        Task<IMappingResponse> Mapping(IMappingRequest request);
    }
}
