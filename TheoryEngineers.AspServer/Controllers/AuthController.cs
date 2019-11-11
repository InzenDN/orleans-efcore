using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheoryEngineers.AspServer.DataTransferObjects;
using TheoryEngineers.Models.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheoryEngineers.AspServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _SignInManager;
        private readonly UserManager<User> _UserManager;
        private readonly IConfiguration _Config;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration config)
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
            _Config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO registerDTO)
        {
            User user = new User
            {
                UserName = registerDTO.UserName,
            };

            IdentityResult result = await _UserManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest();

            string token = await GenerateJwtToken(user);

            return Ok(new { token, user });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            User user = await _UserManager.FindByNameAsync(loginDTO.UserName);
            if (user == null) return Unauthorized();

            SignInResult result = await _SignInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) return Unauthorized();

            string token = await GenerateJwtToken(user);

            return Ok(new { token, user });
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
           };

            IList<string> roles = await _UserManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config.GetSection("AppSettings:Token").Value));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}