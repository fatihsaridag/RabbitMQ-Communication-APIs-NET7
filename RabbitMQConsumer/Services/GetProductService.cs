using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Contexts;
using RabbitMQConsumer.Models;
using RabbitMQConsumer.Repositories;

namespace RabbitMQConsumer.Services
{
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;

        public GetProductService(IProductRepository productRepository)
        {
            _productRepository= productRepository;
        }


        public void GetProduct(Product product)
        {
            _productRepository.ProductAdd(product);
        }
    }
}
