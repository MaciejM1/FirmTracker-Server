using FirmTracker_Server.nHibernate.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using NuGet.Protocol;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportCRUD _reportCRUD;

        public ReportController()
        {
            _reportCRUD = new ReportCRUD();
        }
        [HttpPost]
        [ProducesResponseType(201)] //Created
        [ProducesResponseType(400)] //Bad request
        public IActionResult CreateReport([FromBody] Report.DateRangeDto dateRange)
        {
            try
            {
            
            if (dateRange == null || dateRange.FromDate >= dateRange.ToDate)
            {
                return BadRequest("Invalid date range.");
            }

            var report = _reportCRUD.AddReport(dateRange.FromDate, dateRange.ToDate);
                return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report); // to change?
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetReport(int id)
        {
            var report = _reportCRUD.GetReport(id);
            if (report == null)
                return NotFound();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve // Obsługa cykli obiektów
            };

            var json = JsonSerializer.Serialize(report, options);
            return Ok(json);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllReports()
        {
            var reports = _reportCRUD.GetAllReports();
            if (reports == null)
                return NotFound();

            return Ok(reports);
        }
    }
}
