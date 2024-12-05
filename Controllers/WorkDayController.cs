﻿/*
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

using FirmTracker_Server.Entities;
using FirmTracker_Server.Models;
using FirmTracker_Server.nHibernate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkdayController : ControllerBase
    {
        private readonly WorkdayRepository _workdayCRUD;

        public WorkdayController()
        {
            _workdayCRUD = new WorkdayRepository();
        }

        // Endpoint to start a workday
        [HttpPost("start")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult StartWorkday()
        {
            try
            {
                var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(userIdString);

                // Attempt to start a new workday
                _workdayCRUD.StartWorkday(userId);
                return Ok(new { status = "started", userId });
            }
            catch (Exception ex)
            {
                // If there's an error (like previous workday not stopped), handle it
                return BadRequest(new { message = "An error occurred while starting the workday.", error = ex.Message });
            }
        }
        // Endpoint to stop a workday
        [HttpPost("stop")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult StopWorkday()
        {
            try
            {
                var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(userIdString);

                var result = _workdayCRUD.StopWorkday(userId);
                return Ok(new { status = result ? "stopped" : "already stopped", userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while stopping the workday.", error = ex.Message });
            }
        }



        [HttpGet("user/workdays")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult GetWorkdaysLoggedUser()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var workdays = _workdayCRUD.GetWorkdaysByLoggedUser(userId);
                return Ok(workdays);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching workdays.", error = ex.Message });
            }
        }

        [HttpGet("user/{userMail}/workdays")]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult GetWorkdays(string userMail)
        {
            try
            {
                var workdays = _workdayCRUD.GetWorkdaysByUser(userMail);
                return Ok(workdays);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching workdays.", error = ex.Message });
            }
        }
        
        [HttpGet("absences")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAbsences()
        {
            try
            {
                var absences = _workdayCRUD.GetAbsences();
                return Ok(absences);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching absences.", error = ex.Message });
            }
        }
        [HttpPost("absence/add")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddAbsence([FromBody] string Absence)
        {
            try
            {
                          
                _workdayCRUD.AddAbsence(Absence);

                return Ok(new { status = "added", Absence});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while adding the absence.", error = ex.Message });
            }
        }

        [HttpPost("user/absence/add")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddUserAbsence([FromBody] AddAbsenceDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.userEmail))
                {
                    return BadRequest(new { message = "User email must be provided." });
                }

                // Fetch the userId based on the provided email
                int userId;
                using (var session = SessionFactory.OpenSession())
                {
                    var user = session.Query<User>().FirstOrDefault(u => u.Email == dto.userEmail);
                    if (user == null)
                    {
                        return NotFound(new { message = "User with the given email not found." });
                    }
                    userId = user.UserId;
                }

                // Add the absence for the retrieved userId
                _workdayCRUD.AddAbsenceToUser(userId, dto.AbsenceType, dto.StartTime, dto.EndTime);

                return Ok(new { status = "added", userId, dto.userEmail, absenceType = dto.AbsenceType });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while adding the absence to user.", error = ex.Message });
            }
        }


    }
}
