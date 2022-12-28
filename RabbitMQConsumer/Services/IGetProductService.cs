using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Services
{
    public interface IGetProductService
    {
        void GetProduct(Product product);
    }
}
