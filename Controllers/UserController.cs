/*
 * This file is part of FirmTracker - Server.
 *
 * FirmTracker - Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * FirmTracker - Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with FirmTracker - Server. If not, see <https://www.gnu.org/licenses/>.
 */
using FirmTracker_Server.Models;
using FirmTracker_Server.Services;
using FirmTracker_Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.Entities;
using System.Security.Claims;

using System.Security.Cryptography;
using System.Text;

namespace FirmTracker_Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost("create")]
        //[Authorize(Roles = Roles.Admin)]
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
        [HttpPost("ChangeUserPassword")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult ChangeUserPassword([FromBody] ChangeUserPasswordDto dto)
        {
            try
            {
                var result = UserService.ChangeUserPassword(dto);
                if (result)
                {
                    return Ok("Password changed successfully.");
                }
                else
                {
                    return BadRequest("Failed to change the password.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("changePassword")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public ActionResult ChangePassword([FromBody] UpdatePasswordDto dto)
        {
            try
            {
                var result = UserService.UpdatePassword(dto);
                if (result)
                {
                    var loginDto = new LoginDto { Email = dto.email, Password = dto.newPassword };
                    var token = UserService.CreateTokenJwt(loginDto);
                    return Ok(new { Token = token });
                }
                else
                {
                    return BadRequest("Failed to change the password.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
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
