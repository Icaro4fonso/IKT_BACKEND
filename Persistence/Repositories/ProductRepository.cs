using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Persistence.Context;

namespace IKT_BACKEND.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }
    }
}
