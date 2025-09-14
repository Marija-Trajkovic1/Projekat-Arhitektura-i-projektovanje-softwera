using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskIT.DTOs.UserDTOs;
using TaskIT.Mapping;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository; 
        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet("FindUserForProfile")]
        public async Task<IActionResult> FindUserForProfile()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userRepository.GetAsync(userId);
            if (user == null) 
                return NotFound($"User with not found.");
            var userResponse = user.ToUserProfileDTO();
            return Ok(userResponse);
        }

        [Authorize]
        [HttpPut("UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserRequest updateUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userRepository.UpdateUserAsync(userId, updateUser);
            return Ok(user.ToUserDTO());
        }

        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await userRepository.DeleteAsync(userId);
            return NoContent();
        }
    }
}
