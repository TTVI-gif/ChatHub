using ChatHub.Application.IService;
using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Commons;
using ChatHub.Global.Shared.ViewModel.TokenViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatHub.Infrastructures.Service
{
    public class JwtService : IJwtService
    {
        IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenObjectModel GenerateToken(User user)
        {
            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            string issuer = _configuration["Jwt:Issuer"];
            string audience = _configuration["Jwt:Audience"];
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                //new Claim("role", user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(Constant.JWT_TOKEN_EXPIRITION),
                signingCredentials: credentials
                );

            var expires = DateTimeOffset.Now.AddHours(Constant.JWT_TOKEN_EXPIRITION).ToUnixTimeSeconds();
            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenObject = new TokenObjectModel
            {
                Token = tokenResult,
                Expires = expires
            };
            return tokenObject;
        }
    }
}
