using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string TokenExpire { get; set; }
    }
}
