using IKT_BACKEND.Utils;

namespace IKT_BACKEND.Domain.Services
{
    public interface ISalesService
    {
        BaseResponse<bool> GetSuccess();
        Task<BaseResponse<bool>> SaveRecords(IFormFile file);
    }
}
