using FirmTracker_Server.Models;
using FirmTracker_Server.Services;
using FirmTracker_Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.Entities;
using System.Security.Claims;
using FirmTracker_Server.Exceptions;

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

        [HttpPost("change-password")]
        [Authorize(Roles = Roles.User + "," + Roles.Admin)]
        public ActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // Get the user ID from the claims of the authenticated user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found.");
            }

            try
            {
                // Pass the userId to the service to find the user
                var success = UserService.ChangePassword(userId, dto);
                if (!success)
                {
                    return BadRequest("Password change failed.");
                }

                return Ok("Password changed successfully.");
            }
            catch (WrongUserOrPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost("reset-password")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Reset password for the user
                var success = UserService.ResetPassword(dto.UserMail, dto.NewPassword);
                if (!success)
                {
                    return BadRequest("Password reset failed.");
                }

                return Ok("Password has been successfully reset.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
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
