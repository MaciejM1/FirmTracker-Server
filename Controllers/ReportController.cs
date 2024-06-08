using FirmTracker_Server.nHibernate.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using NuGet.Protocol;
using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate;
using NHibernate.Linq;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportCRUD _reportCRUD;
        private readonly ProductCRUD _productCRUD;

        public ReportController()
        {
            _reportCRUD = new ReportCRUD();
            _productCRUD = new ProductCRUD();
        }
        [HttpPost]
        [ProducesResponseType(201)] //Created
        [ProducesResponseType(400)] //Bad request
        public IActionResult CreateReport([FromBody] Report.DateRangeDto dateRange)
        {
            try
            {
                var fromDate = dateRange.FromDate;
                var toDate = dateRange.ToDate;
                using (var session = SessionFactory.OpenSession())
                {
                    var transactions = session.Query<nHibernate.Transactions.Transaction>()
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .FetchMany(t => t.TransactionProducts)
                        .ToList();

                    var expenses = session.Query<Expense>()
                        .Where(e => e.Date >= fromDate && e.Date <= toDate)
                        .ToList();

                    // Calculate total income from transactions
                    decimal totalIncome = 0;
                    foreach (var transaction in transactions)
                    {
                        foreach (var product in transaction.TransactionProducts)
                        {
                            decimal price = _productCRUD.GetProductPrice(product.ProductID);
                            int type = _productCRUD.GetProductType(product.ProductID);
                            if (type == 1)
                            {
                                totalIncome += ((product.Quantity * price) * ((1 - (transaction.Discount / 100))));
                            }
                            else
                            {
                                totalIncome += (price * ((1 - (transaction.Discount / 100))));
                            }
                        }
                    }

                    var totalExpenses = expenses.Sum(e => e.Value);
                    var totalBalance = totalIncome - totalExpenses;

                    var report = new Report
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        TotalIncome = totalIncome,
                        TotalExpenses = totalExpenses,
                        TotalBalance = totalBalance
                    };

                    _reportCRUD.AddReport(report, transactions, expenses);

                    return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);

                    /*if (dateRange == null || dateRange.FromDate >= dateRange.ToDate)
                    {
                        return BadRequest("Invalid date range.");
                    }

                    var report = _reportCRUD.AddReport(dateRange.FromDate, dateRange.ToDate);
                        return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report); // to change?*/
                }
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
