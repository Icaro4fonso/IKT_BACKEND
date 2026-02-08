using EFCore.BulkExtensions;
using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Dtos;
using IKT_BACKEND.Persistence.Context;
using IKT_BACKEND.Persistence.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<SaleResumeDto>> MostProfitMonthsAsync()
        {
            return await context.Sales
                .GroupBy(sales => new
                {
                    sales.DateTime.Month
                })
                .Select(group => new SaleResumeDto
                {
                    Month = group.Key.Month,
                    TotalProfit = group.Sum(s => s.Price),
                    TotalSalesCount = group.Count()
                })
                .OrderByDescending(sales => sales.TotalProfit)
                .Take(5)
                .ToListAsync();
        }
    }
}
