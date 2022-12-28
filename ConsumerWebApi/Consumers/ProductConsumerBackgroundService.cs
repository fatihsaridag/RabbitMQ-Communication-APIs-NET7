using ConsumerWebApi.Contexts;
using ConsumerWebApi.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;
using System.Text.Json;

namespace ConsumerWebApi.Consumers
{
    public  class ProductConsumerBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMQClientService;
        private IModel _channel;
        private readonly ConsumerWebApiContext _context;

        public ProductConsumerBackgroundService(RabbitMQClientService rabbitMQClientService , ConsumerWebApiContext context)
        {
            _rabbitMQClientService= rabbitMQClientService;
            _context = context;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect(); //Start la beraber bağlantı kurulacak 
            _channel.BasicQos(0, 1, false);             // Ve bize bir bir gönderecek.
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);
            consumer.Received += Consumer_Received;
            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var createMessage = JsonSerializer.Deserialize<Product>(Encoding.UTF8.GetString(@event.Body.ToArray()));
            _context.Products.Add(createMessage);
            _context.SaveChanges();
            Console.WriteLine(createMessage);
            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }


    }
}
