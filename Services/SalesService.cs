using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Utils;

namespace IKT_BACKEND.Services
{
    public class SalesService : ISalesService
    {
        public BaseResponse<bool> GetSuccess()
        {
            return new OkResponse<bool>(true);
        }
    }
}
