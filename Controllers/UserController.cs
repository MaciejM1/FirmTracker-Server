using FirmTracker_Server.Models;
using FirmTracker_Server.Services;
using FirmTracker_Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.Entities;
using System.Security.Claims;

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
        [HttpGet("role")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public ActionResult<string> GetUserRole()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim == null)
            {
                return NotFound("Role not found for the logged-in user.");
            }
            return Ok(roleClaim);
        }
        [HttpGet("emails")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<IEnumerable<string>> GetAllUserEmails()
        {
            var emails = UserService.GetAllUserEmails();
            if (emails == null || !emails.Any())
            {
                return NotFound("No users found or unable to retrieve emails.");
            }

            return Ok(emails);
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
