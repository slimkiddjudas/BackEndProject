using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entity.DTOs
{
    public class OrderDetailPostDto : IDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
