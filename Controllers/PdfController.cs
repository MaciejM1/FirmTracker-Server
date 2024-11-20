using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Transactions;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PdfController(IExpenseRepository expenseRepository, ITransactionRepository transactionRepository)
        {
            _expenseRepository = expenseRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet("download")]
        public IActionResult DownloadReport(
            [FromQuery] string reportType, // "expenses" or "transactions"
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                // Validate date inputs and set default values
                DateTime start = startDate ?? DateTime.MinValue;
                DateTime end = endDate ?? DateTime.MaxValue;

                // Validate report type
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

                        // Main header
                        page.Header()
                            .Text("Raport transakcji")
                            .FontSize(20)
                            .SemiBold()
                            .AlignCenter();

                        // Summary section
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Transakcje od ({startDate:yyyy-MM-dd} do {endDate:yyyy-MM-dd})")
                                .FontSize(16).Underline();

                            // Add table header
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Data").SemiBold();
                                row.RelativeItem().Text("Typ płatności").SemiBold();
                                row.RelativeItem().Text("Kwota razem").SemiBold();
                                row.RelativeItem().Text("Rabat").SemiBold();
                                row.RelativeItem().Text("Opis").SemiBold();
                            });

                            // Populate table rows with transaction data
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

                                // Fetch and display transaction products for this transaction
                                var products = transactionProducts
                                    .Where(tp => tp.TransactionId == transaction.Id)
                                    .ToList();

                                if (products.Any())
                                {
                                    column.Item().Text("Produkty:").SemiBold();
                                    foreach (var product in products)
                                    {
                                        column.Item().Row(productRow =>
                                        {
                                            productRow.RelativeItem().Text($"Nazwa produktu: {product.ProductName}");
                                            productRow.RelativeItem().Text($"Ilość: {product.Quantity}");
                                        });
                                    }
                                }
                            }
                        });

                        // Footer with generation date
                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Wygenerowano przez automat FT: ");
                                text.Span(DateTime.Now.ToString("yyyy-MM-dd")).SemiBold();
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

                        // Main header
                        page.Header()
                            .Text("Raport wydatków")
                            .FontSize(20)
                            .SemiBold()
                            .AlignCenter();

                        // Summary section
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Łączne wydatki: {totalExpenses:C}").FontSize(14).Bold();
                                row.RelativeItem().Text($"Średnie wydatki dzienne: {averageExpense:C}").FontSize(14).Bold();
                            });

                            column.Item().Text($"Szczegóły wydatków od ({startDate:yyyy-MM-dd} do {endDate:yyyy-MM-dd})")
                                .FontSize(16).Underline();

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Data").SemiBold();
                                row.RelativeItem().Text("Kwota").SemiBold();
                                row.RelativeItem().Text("Opis").SemiBold();
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
                                text.Span("Wygenerowano przez automat FT: ");
                                text.Span(DateTime.Now.ToString("yyyy-MM-dd")).SemiBold();
                            });
                    });
                }).GeneratePdf(ms);

                return ms.ToArray();
            }
        }
    }
}
