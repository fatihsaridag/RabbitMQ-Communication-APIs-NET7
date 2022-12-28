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
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };
           
                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.BasicQos(0, 1, false);
                  
                var consumer = new EventingBasicConsumer(channel);
            try
            {

                consumer.Received += (model, e) =>
                {

                        Task.Delay(10000).Wait();

                        var body = e.Body;
                        var message = System.Text.Encoding.UTF8.GetString(body.ToArray());
                        var product = System.Text.Json.JsonSerializer.Deserialize<Product>(message);
                        _logger.LogInformation("Mesajımız : " + product.ProductName);
                        Console.Write($"{product.ProductName}");
                        Product productEntity = new Product
                        {
                            ProductName = product.ProductName,
                            ProductDescription = product.ProductDescription,
                            ProductStock = product.ProductStock,
                            ProductPrice = product.ProductPrice,
                        };
                        _context.Products.AddAsync(productEntity);
                        _context.SaveChangesAsync();

                };
                channel.BasicConsume("product", false, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
    }

