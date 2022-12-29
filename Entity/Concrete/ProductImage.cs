using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.Concrete
{
    public class ProductImage : IEntity
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
