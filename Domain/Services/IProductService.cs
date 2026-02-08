using IKT_BACKEND.Dtos;
using IKT_BACKEND.Utils;

namespace IKT_BACKEND.Domain.Services
{
    public interface IProductService
    {
        Task<BaseResponse<List<ProductResumeDto>>> MostOrdereds();
    }
}
