using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Transactions;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportCRUD
    {
        public Report AddReport(DateTime fromDate, DateTime toDate)
        {
            using (var session = SessionFactory.OpenSession())
            using (var sessionTransaction = session.BeginTransaction())
            {
                try
                {
                    var transactions = session.Query<nHibernate.Transactions.Transaction>()
                        .Where(t => t.Date >= fromDate && t.Date <= toDate)
                        .ToList();

                    var expenses = session.Query<Expense>()
                        .Where(e => e.Date >= fromDate && e.Date <= toDate)
                        .ToList();

                    var totalIncome = transactions.Sum(t => t.TotalPrice);
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

                    foreach (var transactionItem in transactions)
                    {
                        var reportTransaction = new ReportTransaction
                        {
                            ReportId = report.Id,
                            TransactionId = transactionItem.Id
                        };
                        report.ReportTransactions.Add(reportTransaction);
                    }

                    foreach (var expenseItem in expenses)
                    {
                        var reportExpense = new ReportExpense
                        {
                            ReportId = report.Id,
                            ExpenseId = expenseItem.Id
                        };
                        report.ReportExpenses.Add(reportExpense);
                    }

                    session.Save(report);
                    sessionTransaction.Commit();
                    return report;
                }
                catch
                {
                    sessionTransaction.Rollback();
                    throw;
                }
            }
        }

        public Report GetReport(int reportId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<Report>(reportId);
            }
        }

        public IList<Report> GetAllReports()
        {
            using (var session = SessionFactory.OpenSession())
            {
                var reports = session.Query<Report>()
                    .ToList();

                return reports;
            }
        }
    }
}
