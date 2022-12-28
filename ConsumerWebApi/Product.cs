using System.ComponentModel.DataAnnotations.Schema;
using ConsumerWebApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConsumerWebApi
{
    public class Product
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductStock { get; set; }
    }


}
