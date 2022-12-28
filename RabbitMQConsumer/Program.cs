using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQConsumer.Contexts;
using RabbitMQConsumer.Repositories;
using RabbitMQConsumer.ProductBackgroundService;
using RabbitMQConsumer.Services;
using Microsoft.Extensions.Configuration;

namespace RabbitMQConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager _configuration = builder.Configuration;
            IWebHostEnvironment _environment = builder.Environment;
            // Add services to the container.
            builder.Services.AddHostedService<ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IGetProductService,GetProductService>();
            builder.Services.AddHostedService<ProductService>();
            builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(_configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
            builder.Services.AddDbContext<ProductConsumerContext>(opts =>
            {
                opts.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")); 
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}