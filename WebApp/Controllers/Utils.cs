using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InternetForum.WebApp.Controllers
{
    public static class Utils
    {
        public static string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperTajneHaslo111222333"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Name", "Kasia"),
                new Claim(JwtRegisteredClaimNames.Email, "01153047@pw.edu.pl")
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:50001/",
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      

    }
}

