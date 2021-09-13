using CDR.Services.Spreadsheet.Model.Interfaces;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service.Interfaces
{
    public interface IReverseMappingService
    {
        Task<IReverseMappingResponse> ReverseMapping(IReverseMappingRequest request);
    }
}
