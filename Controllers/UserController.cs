using FirmTracker_Server.Models;
using FirmTracker_Server.Services;
using FirmTracker_Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.Entities;

namespace FirmTracker_Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Nieprawidłowa wartość pola. /n" + ModelState);
            }
            var id = UserService.AddUser(dto);
            return Created($"/api/user/{id}", "User dodany poprawnie");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var token = UserService.CreateTokenJwt(dto);
            return Ok(token);
        }
        // New method to get all users
       /* [HttpGet("all")]
        [AllowAnonymous]
        public ActionResult<IList<User>> GetAllUsers()
        {
            var users = UserService.GetAllUsers();
            return Ok(users);
        }*/
    }
}
