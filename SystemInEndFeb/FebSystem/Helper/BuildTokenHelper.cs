using EntityClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FebSystem.Helper
{
    public static class BuildTokenHelper
    {
        private static string BuildToken(IEnumerable<Claim> claims, JwtOptions opts)
        {
            DateTime expires = DateTime.Now.AddSeconds(opts.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(opts.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(expires: expires, signingCredentials: credentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public static string GetToken(IEnumerable<Claim> claims, JwtOptions opts)
        {
            return BuildToken(claims, opts);
        }
    }
}
