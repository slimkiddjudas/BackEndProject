using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Campaign : IEntity
    {
        public int CampaignId { get; set; }
        public int ProductId { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
    }
}
