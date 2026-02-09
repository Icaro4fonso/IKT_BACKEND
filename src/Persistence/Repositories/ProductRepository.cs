using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Dtos;
using IKT_BACKEND.Persistence.Context;
using IKT_BACKEND.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace IKT_BACKEND.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context; 

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<string,long>> FindByRange(HashSet<string> productsNames)
        {
            return await context.Products.Where(p => productsNames.Contains(p.Name))
                                   .ToDictionaryAsync(p => p.Name, p => p.Id);
        }

        public async Task AddByRange(List<Product> products)
        {
             await context.Products.AddRangeAsync(products);
        }

        public async Task<List<ProductResumeDto>> MostOrdereds()
        {
            return await context.Products
             .Select(product => new ProductResumeDto
             {
                 ProductId = product.Id,
                 ProductName = product.Name,
                 TotalQuantity = product.Sales.Count,
                 TotalRevenue = product.Sales.Sum(s => s.Price)
             })
             .OrderByDescending(p => p.TotalQuantity)
             .Take(3)
             .ToListAsync();
        }

    }
}
