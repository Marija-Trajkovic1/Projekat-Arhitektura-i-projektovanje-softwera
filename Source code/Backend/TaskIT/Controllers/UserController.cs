using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        [HttpGet("FindUserById/{id}")]
        public async Task<IActionResult> FindUserById(string id)
        {
            var user = await userRepository.GetAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            var userResponse = user.ToUserDTO();
            return Ok(userResponse);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUser)
        {
            if (createUser == null)
            {
                return BadRequest("User data is null.");
            }
            var user = createUser.ToUserFromCreateUserRequest();
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            await userRepository.CreateAsync(user);
            return CreatedAtAction(nameof(FindUserById), new { id = user.Id }, user.ToUserDTO());
        }

        [HttpPut("UpdateUserInformation/{id}")]
        public async Task<IActionResult> UpdateUserInformation([FromRoute] string id, [FromBody] UpdateUserRequest updateUser)
        {
            var userForUpdate = updateUser.ToUserFromUpdateUserRequest(id);
            var user = await userRepository.UpdateAsync(id, userForUpdate);
            await unitOfWork.CompleteAsync();
            return Ok(user.ToUserDTO());

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
