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
            _workdayCRUD = new WorkdayRepository(); // Instantiate directly (no DI in this example)
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
                _workdayCRUD.StartWorkday(userId);
                return Ok(new { status = "started", userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while starting the workday.", error = ex.Message });
            }
        }

      
    }
}
