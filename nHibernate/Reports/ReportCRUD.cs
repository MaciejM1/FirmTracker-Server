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
        public Report AddReport(Report report, IList<nHibernate.Transactions.Transaction> transactions, IList<Expense> expenses /*DateTime fromDate, DateTime toDate*/)
        {
            using (var session = SessionFactory.OpenSession())
            using (var sessionTransaction = session.BeginTransaction())
            {
                try
                {
                    /*var transactions = session.Query<nHibernate.Transactions.Transaction>()
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

                    };*/

                    session.Save(report);

                    foreach (var transactionItem in transactions)
                    {
                        //var trans = session.Load<nHibernate.Transactions.Transaction>(transactionItem);
                        var reportTransaction = new ReportTransaction
                        {
                            Report = report,
                            Transaction = transactionItem
                        };
                        session.Save(reportTransaction);
                        //report.ReportTransactions.Add(reportTransaction);
                        //report.TotalIncome += trans.TotalPrice;
                    }

                    foreach (var expenseItem in expenses)
                    {
                        //var expense = session.Load<Expense>(expenseItem);
                        var reportExpense = new ReportExpense
                        {
                            Report = report,
                            Expense = expenseItem
                        };
                        session.Save(reportExpense);
                        //report.TotalExpenses += expense.Value;
                        //report.ReportExpenses.Add(reportExpense);
                    }


                    //session.Save(report);
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
