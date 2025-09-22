using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskIT.Constants;
using TaskIT.DTOs.AuthenticateUserDTOs;
using TaskIT.Mapping;
using TaskIT.Services;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
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
            var role= (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var userResponse = user.ToUserDTO();
            return Ok(new { Token = token, UserResponse=userResponse, Role=role});
        }

        [Authorize]
        [HttpPost("ChangeRole")]
        public async Task<IActionResult> ChangeRole(string currentRole)
        {
            var userId = User.GetUserId();
            var user= await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var currentRoles = await userManager.GetRolesAsync(user);

            if (!currentRoles.Contains(currentRole))
            {
                return BadRequest($"User does not have role: {currentRole}");
            }

            if (currentRoles.Contains(currentRole))
            {
                var removeResult = await userManager.RemoveFromRoleAsync(user, currentRole);
                if (!removeResult.Succeeded)
                {
                    return BadRequest($"Failed to remove role {currentRole}: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                }

                string newRole = currentRole == "WORKER" ? "EMPLOYER" : "WORKER";
                var addResult = await userManager.AddToRoleAsync(user, newRole);
                if (!addResult.Succeeded)
                {
                    return BadRequest($"Failed to add role {newRole}: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
                }
            }

            var token = await tokenService.CreateTokenAsync(user);
            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var userChanged = await userManager.FindByIdAsync(userId);
            var userResponse = userChanged.ToUserDTO();
            return Ok(new { Token = token, UserResponse = userResponse, Role = role });
        }

    }
}
