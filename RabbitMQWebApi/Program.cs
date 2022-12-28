
using Microsoft.EntityFrameworkCore;
using RabbitMQWebApi.Data;
using RabbitMQWebApi.RabbitMQ;
using RabbitMQWebApi.Services;

namespace RabbitMQWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager _configuration = builder.Configuration;

            // Add services to the container.


            builder.Services.AddScoped<IProductService ,ProductService>();
            builder.Services.AddScoped<IRabbitMQProducer,RabbitMQProducer>();
            builder.Services.AddDbContext<DbContextClass>(opts =>
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