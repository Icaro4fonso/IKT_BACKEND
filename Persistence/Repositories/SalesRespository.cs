using EFCore.BulkExtensions;
using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Persistence.Context;
using IKT_BACKEND.Persistence.Models;

namespace IKT_BACKEND.Persistence.Repositories
{
    public class SalesRespository : ISalesRespository
    {
        private readonly AppDbContext context;

        public SalesRespository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task BulkInsertAsync(List<Sale> sales) 
        {
            await context.BulkInsertOrUpdateAsync(sales, options =>
            {
                options.BatchSize = 5000;
                options.UpdateByProperties =
                [
                    nameof(Sale.DateTime),
                    nameof(Sale.ProductId),
                    nameof(Sale.Price)
                ];
            });
        }
    }
}
