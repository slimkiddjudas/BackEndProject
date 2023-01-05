using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs
{
    public class ProductDetailDto : IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public List<ProductImage> ImageUrls { get; set; }
        public string Description { get; set; }
    }
}
