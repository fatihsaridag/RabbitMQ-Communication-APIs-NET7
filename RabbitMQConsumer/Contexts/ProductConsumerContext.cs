using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Contexts
{
    public class ProductConsumerContext : DbContext
    {

        public ProductConsumerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }


    }
}
