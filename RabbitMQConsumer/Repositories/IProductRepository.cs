using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Repositories
{

    public interface IProductRepository
    {
        void ProductAdd(Product product);
    }
}
