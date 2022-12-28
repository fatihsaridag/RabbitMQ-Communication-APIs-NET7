using Microsoft.EntityFrameworkCore;

namespace ConsumerWebApi.Contexts
{
    public class ConsumerWebApiContext : DbContext
    {
        
        public ConsumerWebApiContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
