using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Contexts;
using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductConsumerContext _context;

        public ProductRepository(ProductConsumerContext context)
        {
            _context = context;
        }

        public void ProductAdd(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();

        }
    }
}
