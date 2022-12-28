using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQWebApi.Models;

namespace RabbitMQWebApi.Data
{
    public class DbContextClass:  DbContext
    {
        public DbContextClass(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

    }
}
