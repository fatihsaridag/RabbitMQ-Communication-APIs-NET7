
using ConsumerWebApi.Consumers;
using ConsumerWebApi.Contexts;
using ConsumerWebApi.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace ConsumerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager _configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddHostedService<ProductConsumerBackgroundService>();
            builder.Services.AddSingleton<RabbitMQClientService>();
            builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(_configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
            builder.Services.AddDbContext<ConsumerWebApiContext>(opts =>
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

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}