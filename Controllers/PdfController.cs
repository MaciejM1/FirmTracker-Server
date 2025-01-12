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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PdfController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;

        public PdfController(IExpenseRepository expenseRepository, ITransactionRepository transactionRepository, IProductRepository productRepository)
        {
            _expenseRepository = expenseRepository;
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
        }

        [HttpGet("download")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DownloadReport(
            [FromQuery] string reportType, // "expenses" or "transactions"
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                DateTime start = startDate ?? DateTime.MinValue;
                DateTime end = endDate ?? DateTime.MaxValue;

                if (string.IsNullOrEmpty(reportType) ||
                    (reportType.ToLower() != "expenses" && reportType.ToLower() != "transactions"))
                {
                    return BadRequest("Invalid report type. Please specify 'expenses' or 'transactions'.");
                }

                if (reportType.ToLower() == "expenses")
                {
                    return GenerateExpenseReport(start, end);
                }
                else
                {
                    return GenerateTransactionReport(start, end);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private IActionResult GenerateExpenseReport(DateTime start, DateTime end)
        {
            var expenses = _expenseRepository.GetAllExpenses()
                .Where(e => e.Date >= start && e.Date <= end)
                .ToList();

            if (!expenses.Any())
            {
                return BadRequest($"No expenses found between {start:yyyy-MM-dd} and {end:yyyy-MM-dd}.");
            }

            var pdfBytes = GenerateExpensePdf(expenses, start, end);
            string fileName = $"ExpenseReport_{start:yyyy-MM-dd}_to_{end:yyyy-MM-dd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        private IActionResult GenerateTransactionReport(DateTime start, DateTime end)
        {
            var transactions = _transactionRepository.GetTransactionsByDateRange(start, end);

            if (!transactions.Any())
            {
                return BadRequest($"No transactions found between {start:yyyy-MM-dd} and {end:yyyy-MM-dd}.");
            }

            // Fetch transaction products for all transactions in one query
            var transactionIds = transactions.Select(t => t.Id).ToList();
            var transactionProducts = _transactionRepository.GetTransactionProductsForTransactions(transactionIds);

            var pdfBytes = GenerateTransactionPdf(transactions, transactionProducts, start, end);
            string fileName = $"TransactionReport_{start:yyyy-MM-dd}_to_{end:yyyy-MM-dd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        private byte[] GenerateTransactionPdf(List<Transaction> transactions, List<TransactionProduct> transactionProducts, DateTime startDate, DateTime endDate)
        {
            using (var ms = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));
                        page.Header()
                            .Text("Raport transakcji")
                            .FontSize(22)
                            .SemiBold()
                            .FontColor(Colors.Blue.Medium)
                            .AlignCenter();

                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Transakcje od ({startDate:yyyy-MM-dd} do {endDate:yyyy-MM-dd})")
                                .FontSize(16)
                                .Underline()
                                .FontColor(Colors.Grey.Medium);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Data").SemiBold().FontColor(Colors.Blue.Darken1);
                                row.RelativeItem().Text("Typ płatności").SemiBold().FontColor(Colors.Blue.Darken1);
                                row.RelativeItem().Text("Kwota razem").SemiBold().FontColor(Colors.Blue.Darken1);
                                row.RelativeItem().Text("Rabat").SemiBold().FontColor(Colors.Blue.Darken1);
                                row.RelativeItem().Text("Opis").SemiBold().FontColor(Colors.Blue.Darken1);
                            });
                            foreach (var transaction in transactions)
                            {
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(transaction.Date.ToString("yyyy-MM-dd"));
                                    row.RelativeItem().Text(transaction.PaymentType);
                                    row.RelativeItem().Text(transaction.TotalPrice.ToString("C"));
                                    row.RelativeItem().Text(transaction.Discount.ToString("C"));
                                    row.RelativeItem().Text(transaction.Description);
                                });
                                var products = transactionProducts
                                    .Where(tp => tp.TransactionId == transaction.Id)
                                    .ToList();

                                if (products.Any())
                                {
                                    column.Item().Text("Produkty:").SemiBold().FontColor(Colors.Blue.Medium);
                                    foreach (var product in products)
                                    {
                                        var productQuery = _productRepository.GetProduct(product.Id);
                                        column.Item().Row(productRow =>
                                        {
                                            productRow.RelativeItem().Text($"Nazwa produktu: {productQuery.Name}");
                                            productRow.RelativeItem().Text($"Ilość: {product.Quantity}");
                                            productRow.RelativeItem().Text($"Cena 1 szt. bez rabatu: {productQuery.Price.ToString("F2")}");
                                        });
                                    }
                                }
                            }
                        });
                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Wygenerowano przez automat FT: ").FontColor(Colors.Grey.Medium);
                                text.Span(DateTime.Now.ToString("yyyy-MM-dd")).SemiBold().FontColor(Colors.Grey.Medium);
                            });
                    });
                }).GeneratePdf(ms);

                return ms.ToArray();
            }
        }

        private byte[] GenerateExpensePdf(List<Expense> expenses, DateTime startDate, DateTime endDate)
        {
            using (var ms = new MemoryStream())
            {
                decimal totalExpenses = expenses.Sum(e => e.Value);
                decimal averageExpense = expenses.Any() ? totalExpenses / expenses.Count : 0;

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));
                        page.Header()
                            .Text("Raport wydatków")
                            .FontSize(22)
                            .SemiBold()
                            .FontColor(Colors.Green.Medium)
                            .AlignCenter();
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Łączne wydatki: {totalExpenses:C}").FontSize(14).Bold().FontColor(Colors.Green.Darken1);
                                row.RelativeItem().Text($"Średnie wydatki dzienne: {averageExpense:C}").FontSize(14).Bold().FontColor(Colors.Green.Darken1);
                            });

                            column.Item().Text($"Szczegóły wydatków od ({startDate:yyyy-MM-dd} do {endDate:yyyy-MM-dd})")
                                .FontSize(16)
                                .Underline()
                                .FontColor(Colors.Grey.Medium);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Data").SemiBold().FontColor(Colors.Green.Darken1);
                                row.RelativeItem().Text("Kwota").SemiBold().FontColor(Colors.Green.Darken1);
                                row.RelativeItem().Text("Opis").SemiBold().FontColor(Colors.Green.Darken1);
                            });

                            foreach (var expense in expenses)
                            {
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(expense.Date.ToString("yyyy-MM-dd"));
                                    row.RelativeItem().Text(expense.Value.ToString("C"));
                                    row.RelativeItem().Text(expense.Description);
                                });
                            }
                        });
                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Wygenerowano przez automat FT: ").FontColor(Colors.Grey.Medium);
                                text.Span(DateTime.Now.ToString("yyyy-MM-dd")).SemiBold().FontColor(Colors.Grey.Medium);
                            });
                    });
                }).GeneratePdf(ms);

                return ms.ToArray();
            }
        }


    }
}
