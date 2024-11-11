using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Expenses;
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

        public PdfController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet("download")]
        public IActionResult DownloadExpenseReport()
        {
            // Fetch expense data from the repository
            List<Expense> expenses = _expenseRepository.GetAllExpenses();

            // Generate the PDF file
            byte[] pdfBytes = GeneratePdf(expenses);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = $"ExpenseReport_{date}.pdf";

            // Return the PDF as a downloadable file
            return File(pdfBytes, "application/pdf", fileName);
        }

        private byte[] GeneratePdf(List<Expense> expenses)
        {
            using (var ms = new MemoryStream())
            {
                // Calculate total and average expenses
                decimal totalExpenses = expenses.Sum(e => e.Value);
                decimal averageExpense = expenses.Count > 0 ? totalExpenses / expenses.Count : 0;

                // Define the document using QuestPDF
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
                            .Text("Expense Report")
                            .FontSize(20)
                            .SemiBold()
                            .AlignCenter();

                        // Summary section
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Total Expenses: {totalExpenses:C}").FontSize(14).Bold();
                                row.RelativeItem().Text($"Average Expense: {averageExpense:C}").FontSize(14).Bold();
                            });

                            column.Item().Text("Expense Details").FontSize(16).Underline();

                            // Add a table header
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Date").SemiBold();
                                row.RelativeItem().Text("Value").SemiBold();
                                row.RelativeItem().Text("Description").SemiBold();
                            });

                            // Populate table rows with expense data
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

                        // Footer with generation date
                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Generated on ");
                                text.Span(DateTime.Now.ToString("yyyy-MM-dd")).SemiBold();
                            });
                    });
                }).GeneratePdf(ms);

                return ms.ToArray();
            }
        }
    }
}
