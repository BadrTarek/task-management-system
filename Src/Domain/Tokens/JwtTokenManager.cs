using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Tokens
{
    public class JwtTokenManager : ITokenManager
    {
        private readonly string _jwtSecretKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly DateTime _expirationTime;

        public JwtTokenManager(string jwtSecretKey, string jwtIssuer, string jwtAudience, DateTime expirationTime)
        {
            _jwtSecretKey = jwtSecretKey;
            _jwtIssuer = jwtIssuer;
            _jwtAudience = jwtAudience;
            _expirationTime = expirationTime;
        }
        public string GenerateToken(string id, string email, string name)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: _expirationTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}