using IKT_BACKEND.Persistence.Models;

namespace IKT_BACKEND.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Dictionary<string, long>> FindByRange(HashSet<string> productsNames);
       Task AddByRange(List<Product> products);
    }
}
