using Microsoft.AspNetCore.Mvc;
using RabbitMQWebApi.Models;
using RabbitMQWebApi.RabbitMQ;
using RabbitMQWebApi.Services;

namespace RabbitMQWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IRabbitMQProducer _rabbitMQProducer;


        public ProductController(IProductService productService , IRabbitMQProducer rabbitMQProducer)
        {
            _productService = productService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpGet("getproductbyid")]
        public Product GetProductById(int id)
        {
          return  _productService.GetProductById(id);
        }

        [HttpPost("addproduct")]
        public Product AddProduct(Product product)
        {
            var productData = _productService.AddProduct(product);
            //Eklenen ürün verisi kuyruga gönderildi ve  tüketici kuyruktan bu veriyi dinleyecek.
            _rabbitMQProducer.SendProductMessage(productData);
            return productData;
        }

        [HttpPut("updateproduct")]
        public Product UpdateProduct(Product product)
        {
            var productData = _productService.UpdateProduct(product);
            return productData;
        }

        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int Id)   
        {
            return _productService.DeleteProduct(Id);
        }

        [HttpGet("productlist")]
        public IEnumerable<Product> ProductList()
        {
            var productList = _productService.GetProductList();
            return productList;
        }


    }
}
