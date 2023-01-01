using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Models
{
    public class Token
    {
        public string TokenBody { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
    }
}
