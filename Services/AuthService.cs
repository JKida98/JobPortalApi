using System;
using System.Threading.Tasks;
using JobPortalApi.Configurations;
using JobPortalApi.Database.Models;
using JobPortalApi.Models;
using JobPortalApi.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace JobPortalApi.Services
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(UserForRegistration user);
        Task<string> LoginUserAsync(UserForLogin user);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        private readonly AppSettings _appSettings;

        public AuthService(UserManager<User> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<string> RegisterUserAsync(UserForRegistration user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                throw new Exception("Unable to create a user");
            }

            var newUser = new User() {Email = user.Email, UserName = user.Username};
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                return GenerateJwtToken(newUser);
            }

            throw new Exception("Creation of a user did not succeed");
        }

        public async Task<string> LoginUserAsync(UserForLogin user)
        {
            var found = await _userManager.FindByEmailAsync(user.Email);

            if (found == null)
            {
                throw new Exception("User with the credentials does not exist");
            }

            var isCorrect = await _userManager.CheckPasswordAsync(found, user.Password);

            if (!isCorrect)
            {
                throw new Exception("Invalid credentials for a user");
            }

            return GenerateJwtToken(found);
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(360),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}