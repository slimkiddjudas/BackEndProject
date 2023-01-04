using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entity.DTOs
{
    public class OrderDto : IDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserMail { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
