using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkdayController : ControllerBase
    {
        // In-memory storage for simplicity, where the key is the userId.
        private static readonly ConcurrentDictionary<string, DateTime?> WorkStartTimes = new ConcurrentDictionary<string, DateTime?>();

        // Get the current status of the user's workday (started or not)
        [HttpGet("status/{userId}")]
        public IActionResult GetWorkdayStatus(string userId)
        {
            if (WorkStartTimes.TryGetValue(userId, out DateTime? startTime))
            {
                if (startTime.HasValue)
                {
                    return Ok(new { status = "started", startTime = startTime });
                }
                else
                {
                    return Ok(new { status = "stopped" });
                }
            }
            else
            {
                return NotFound(new { message = "User not found" });
            }
        }

        // Start or stop the user's workday by toggling the start/stop state
        [HttpPost("toggle/{userId}")]
        public IActionResult ToggleWorkday(string userId)
        {
            // If the workday has already started, stop it, otherwise start it
            if (WorkStartTimes.ContainsKey(userId) && WorkStartTimes[userId].HasValue)
            {
                // Stop the workday
                WorkStartTimes[userId] = null;
                return Ok(new { status = "stopped" });
            }
            else
            {
                // Start the workday
                WorkStartTimes[userId] = DateTime.Now;
                return Ok(new { status = "started", startTime = WorkStartTimes[userId] });
            }
        }
    }
}
