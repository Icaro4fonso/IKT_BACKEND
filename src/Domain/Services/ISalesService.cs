using IKT_BACKEND.Dtos;
using IKT_BACKEND.Utils;

namespace IKT_BACKEND.Domain.Services
{
    public interface ISalesService
    {
        Task<BaseResponse<List<string>>> SaveRecords(IFormFile file);
        Task<BaseResponse<List<SaleResumeDto>>> MostProfitMonths();
    }
}
