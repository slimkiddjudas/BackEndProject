using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs
{
    public class ProductDetailDto
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
