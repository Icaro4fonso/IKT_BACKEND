using IKT_BACKEND.Dtos;
using IKT_BACKEND.Persistence.Models;

namespace IKT_BACKEND.Domain.Repositories
{
    public interface ISalesRespository
    {
        Task BulkInsertAsync(List<Sale> sales);
        Task<List<SaleResumeDto>> MostProfitMonthsAsync();
    }
}
