using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskIT.Services
{
    public class TokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        
        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"]),

            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            //debuuug
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = configuration["Jwt:Key"];

            Console.WriteLine($"Issuer: {issuer}, Audience: {audience}, Key: {key}");

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT configuration is missing in appsettings.json");
            }

            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var token = new JwtSecurityToken (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
