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

using FirmTracker_Server.nHibernate.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using NuGet.Protocol;
using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate;
using NHibernate.Linq;
using Microsoft.AspNetCore.Authorization;


namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult CreateReport([FromBody] Report.DateRangeDto dateRange)
        {
            try
            {
                var fromDate = dateRange.FromDate;
                var toDate = dateRange.ToDate;
                if (fromDate >= toDate)
                {
                    return BadRequest("Nieprawidłowy zakres dat.");
                }
                using (var session = SessionFactory.OpenSession())
                {
                    var transactions = session.Query<nHibernate.Transactions.Transaction>()
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .FetchMany(t => t.TransactionProducts)
                        .ToList();

                    var expenses = session.Query<Expense>()
                        .Where(e => e.Date >= fromDate && e.Date <= toDate)
                        .ToList();


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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetReport(int id)
        {
            var report = _reportCRUD.GetReport(id);
            if (report == null)
                return NotFound();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(report, options);
            return Ok(json);
        }

        [HttpGet("{id}/transactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetReportTransactions(int id)
        {
            var transactions = _reportCRUD.GetReportTransactions(id);
            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        [HttpGet("{id}/expenses")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetReportExpenses(int id)
        {
            var expenses = _reportCRUD.GetReportExpenses(id);
            if (expenses == null)
            {
                return NotFound();
            }
            return Ok(expenses);
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAllReports()
        {
            var reports = _reportCRUD.GetAllReports();
            if (reports == null)
                return NotFound();

            return Ok(reports);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpdateReport(int id, [FromBody] Report.DateRangeDto dateRange)
        {
            try
            {
                var report = _reportCRUD.GetReport(id);
                if (report == null)
                {
                    return NotFound();
                }
                using (var session = SessionFactory.OpenSession())
                {
                    var fromDate = dateRange.FromDate;
                    var toDate = dateRange.ToDate;
                    if (fromDate >= toDate)
                    {
                        return BadRequest("Nieprawidłowy zakres dat.");
                    }
                    var transactions = session.Query<nHibernate.Transactions.Transaction>()
                .Where(t => t.Date >= fromDate && t.Date <= toDate)
                .FetchMany(t => t.TransactionProducts)
                .ToList();

                    var expenses = session.Query<Expense>()
                        .Where(e => e.Date >= fromDate && e.Date <= toDate)
                        .ToList();


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

                    report.FromDate = fromDate;
                    report.ToDate = toDate;
                    report.TotalIncome = totalIncome;
                    report.TotalExpenses = totalExpenses;
                    report.TotalBalance = totalBalance;

                    _reportCRUD.UpdateReport(report, transactions, expenses);

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteReport(int id)
        {
            try
            {
                _reportCRUD.DeleteReport(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
