﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs
{
    public class OrderDetailDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
