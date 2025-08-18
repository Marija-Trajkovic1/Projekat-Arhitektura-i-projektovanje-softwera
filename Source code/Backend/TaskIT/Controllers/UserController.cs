using Microsoft.AspNetCore.Mvc;
using TaskIT.DTOs.UserDTOs; 
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
        private readonly TaskITContext context;//kad koristim repository ne treba mi context! ne zelimo direktan pristup bazi u kontroleru

        private readonly UserRepository userRepository; 
        public UnitOfWorkImpl unitOfWork { get; set; }

        public UserController(TaskITContext context, UserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.context = context;
            unitOfWork = new UnitOfWorkImpl(context);

        }

        [HttpGet("FindAllUsers")]
        public async Task<IActionResult> FindAllUsers()
        {
            var users = await userRepository.GetAllAsync();
            var usersDTO=    users.Select(s => s.ToUserDTO());
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
            return Ok(user.ToUserDTO());
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest("User data is null.");
            }
            var user = userCreateDto.ToUserFromCreateUserRequest();
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            await userRepository.CreateAsync(user);
            return CreatedAtAction(nameof(FindUserById), new { id = user.Id }, user.ToUserDTO());
        }

        [HttpPut("UpdateUserInformation/{id}")]
        public async Task<IActionResult> UpdateUserInformation([FromRoute] string id, [FromBody] UpdateUserRequest userUpdateDto)
        {
            var userForUpdate = userUpdateDto.ToUserFromUpdateUserRequest(id);
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
