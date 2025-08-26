using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TaskIT.DTOs.UserDTOs;
using TaskIT.Hubs;
using TaskIT.Mapping;
using TaskIT.Model;
using TaskIT.Repository.UnityOfWork;
using TaskIT.Repository.UserRepositoryF;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository; 
        private readonly IHubContext<TaskItHub> hubContext;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public UserController(TaskITContext context, UserRepository userRepository, IHubContext<TaskItHub> hubContext)
        {
            this.userRepository = userRepository;
            this.hubContext = hubContext;
            unitOfWork = new UnitOfWorkImpl(context);
        }

        [Authorize]
        [HttpGet("FindAllUsers")]
        public async Task<IActionResult> FindAllUsers()
        {
            var users = await userRepository.GetAllAsync();
            var usersDTO = users.Select(s => s.ToUserDTO());
            if (usersDTO == null || !usersDTO.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(usersDTO);
        }

        [Authorize]
        [HttpGet("FindUser")]
        public async Task<IActionResult> FindUser()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userRepository.GetAsync(userId);
            if (user == null)
            {
                return NotFound($"User with not found.");
            }
            var userResponse = user.ToUserDTO();
            return Ok(userResponse);
        }

        [Authorize]
        [HttpPut("UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserRequest updateUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userForUpdate = updateUser.ToUserFromUpdateUserRequest(userId);
            var user = await userRepository.UpdateAsync(userId, userForUpdate);
            await unitOfWork.CompleteAsync();
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
