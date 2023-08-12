using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Security
{
    public static class TokenSecurity
    {
        public static Object[] JwtToken(UserDTO user, string _secretKey, string _url)
        {
            var expireDate = DateTime.UtcNow.AddHours(3);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var secretBytes = Generate128BitKey();
            byte[] byteArray = Encoding.UTF8.GetBytes(secretBytes);
            var key = new SymmetricSecurityKey(byteArray);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                issuer: _url,
                audience: _url,
                claims, expires: expireDate,
                signingCredentials: signingCredentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return new object[] { tokenJson, expireDate };
        }

        public static string Generate128BitKey()
        {
            // Generate 16 bytes (128 bits) of random data
            byte[] randomBytes = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the random bytes to a hexadecimal string
            string hexKey = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();

            return hexKey;
        }
    }
}
