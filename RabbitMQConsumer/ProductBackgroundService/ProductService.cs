using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQConsumer.Models;
using RabbitMQConsumer.Repositories;
using RabbitMQConsumer.Contexts;
using System.Data;
using RabbitMQConsumer.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Dtos;
using System.Threading.Channels;

namespace RabbitMQConsumer.ProductBackgroundService
{
   

    public class ProductService : BackgroundService
    {       
     

        private readonly ILogger<ProductService> _logger;
        private readonly ProductConsumerContext _context;


        public ProductService(ILogger<ProductService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ProductConsumerContext>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://eznbdupx:Qf4h0Avxf0yEipy5VaR1D7UHRfIL0Gfn@gerbil.rmq.cloudamqp.com/eznbdupx");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("product", false, consumer);
         
                consumer.Received += (object sender , BasicDeliverEventArgs e) =>
                {
                        var message = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                        var product = System.Text.Json.JsonSerializer.Deserialize<Product>(message);
                   
                        Product productEntity = new Product
                        {
                            ProductName = product.ProductName,
                            ProductDescription = product.ProductDescription,
                            ProductStock = product.ProductStock,
                            ProductPrice = product.ProductPrice,
                        };
                        _context.Products.AddAsync(productEntity);
                        _context.SaveChangesAsync();
                        channel.BasicAck(e.DeliveryTag, false);
                };
            Console.WriteLine("İşlem başarılı");
            //Solution is here
            Console.ReadKey();
            return Task.CompletedTask;
        }
    }
}

