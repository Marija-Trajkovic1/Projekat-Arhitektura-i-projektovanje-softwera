using Microsoft.AspNetCore.Mvc;
using TaskIT.Mapping;
using TaskIT.Model;
using TaskIT.Repository.UnityOfWork;
using TaskIT.DTOs;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public UserController(TaskITContext context)
        {
            this.context = context;
            unitOfWork = new UnitOfWorkImpl(context);

        }

        [HttpGet("FindAllUsers")]
        public async Task<IActionResult> FindAllUsers()
        {
            var users = await context.Users.ToListAsync();
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
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user.ToUserDTO());
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest("User data is null.");
            }
            var user =  userCreateDto.ToUserFromCreateUserRequest();
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(FindUserById), new { id = user.Id }, user.ToUserDTO());
        }

        [HttpPut("UpdateUserInformation/{id}")]
        public async Task<IActionResult> UpdateUserInformation([FromRoute] string id, [FromBody] UpdateUserRequestDTO userUpdateDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            user.Name = userUpdateDto.Name;
            user.Surname = userUpdateDto.Surname;
            user.Email = userUpdateDto.Email;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.HomeNumber = userUpdateDto.HomeNumber;
            user.City = userUpdateDto.City;
            user.Street = userUpdateDto.Street;

            await unitOfWork.CompleteAsync();
            return Ok(user.ToUserDTO());

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            unitOfWork.Users.Remove(user);
            await unitOfWork.CompleteAsync();
            return NoContent();

        }
    }
}
