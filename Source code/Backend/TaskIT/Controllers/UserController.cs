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
        public IActionResult FindAllUsers()
        {
            var users = context.Users.ToList()
                .Select(s=>s.ToUserDTO());
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("FindUserById/{id}")]
        public async  Task<IActionResult> FindUserById(string id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user.ToUserDTO());
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] CreateUserRequestDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null.");
            }
            var user = userDto.ToUserFromCreateUserRequest();
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();
            return CreatedAtAction(nameof(FindUserById), new { id = user.Id }, user.ToUserDTO());
        }

    }
}
