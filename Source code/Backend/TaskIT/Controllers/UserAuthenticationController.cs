using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.DTOs.AuthenticateUserDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.UserRepositoryF;
using TaskIT.Services;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly TokenService tokenService;

        public UserAuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpPost("RegisterNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterNewUserRequest registerNewUserDTO)
        {
            var user = registerNewUserDTO.ToUserFromRegisterNewUserRequest();
            var result= await userManager.CreateAsync(user, registerNewUserDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, registerNewUserDTO.Role);

            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequesDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequesDTO.Email);
            if (user == null) return Unauthorized("Invalid credentials!");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginRequesDTO.Password, false);
            if(!result.Succeeded) return Unauthorized("Invalid email or password");

            var token = await tokenService.CreateTokenAsync(user);

            return Ok(new { Token = token});
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(userId);
        }
    }
}
