using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RabbitMQWebApi.Data;
using RabbitMQWebApi.Models;

namespace RabbitMQWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly DbContextClass _context;
        public ProductService(DbContextClass context)
        {
            _context= context;
        }

        public Product AddProduct(Product product)
        {
            var result = _context.Products.Add(product);
            _context.SaveChanges();
            return result.Entity;
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            var result = _context.Remove(product);
            _context.SaveChanges();
            return result != null ? true : false;
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return product;
        }

        public IEnumerable<Product> GetProductList()
        {
            var products = _context.Products.ToList();
            return products;
        }

        public Product UpdateProduct(Product product)
        {
             var result =  _context.Products.Update(product);
            _context.SaveChanges();
            return result.Entity;
        }
    }
}
