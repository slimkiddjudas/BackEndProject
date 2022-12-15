using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Cart : IEntity
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}
