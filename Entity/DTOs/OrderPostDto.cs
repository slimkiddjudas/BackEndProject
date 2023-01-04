using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Entity.Concrete;

namespace Entity.DTOs
{
    public class OrderPostDto : IDto
    {
        public string? UserId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderDetailPostDto> OrderDetails { get; set; }
    }
}
