using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Dtos;
using IKT_BACKEND.Utils;

namespace IKT_BACKEND.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<BaseResponse<List<ProductResumeDto>>> MostOrdereds()
        {
            List<ProductResumeDto> products = await productRepository.MostOrdereds();

            return new OkResponse<List<ProductResumeDto>>(products);
        }
    }
}
