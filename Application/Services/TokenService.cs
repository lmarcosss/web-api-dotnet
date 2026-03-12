using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Application.Services.Interfaces;
using WebApi.Domain.Models;
using WebApi.Settings;

namespace WebApi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;

        public TokenService(IOptions<JwtSettings> options)
        {
            _secret = options.Value.ApiSecret;
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", user.id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);

            return tokenHandler.WriteToken(token);
        }
    }
}