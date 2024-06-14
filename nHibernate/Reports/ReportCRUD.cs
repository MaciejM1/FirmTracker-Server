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


        public IList<nHibernate.Transactions.Transaction> GetReportTransactions(int reportId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<ReportTransaction>()
                              .Where(rt => rt.Report.Id == reportId)
                              .Select(rt => rt.Transaction)
                              .Fetch(t => t.TransactionProducts)
                              .ToList();
            }
        }

        public IList<Expense> GetReportExpenses(int reportId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<ReportExpense>()
                              .Where(re => re.Report.Id == reportId)
                              .Select(re => re.Expense)
                              .ToList();
            }
        }

        public Report UpdateReport(Report report, IList<nHibernate.Transactions.Transaction> transactions, IList<Expense> expenses)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Update(report);


                    session.CreateQuery("delete from ReportTransaction where Report.Id = :reportId")
                           .SetParameter("reportId", report.Id)
                           .ExecuteUpdate();
                    session.CreateQuery("delete from ReportExpense where Report.Id = :reportId")
                           .SetParameter("reportId", report.Id)
                           .ExecuteUpdate();

                    foreach (var transactionItem in transactions)
                    {
                        var reportTransaction = new ReportTransaction
                        {
                            Report = report,
                            Transaction = transactionItem
                        };
                        session.Save(reportTransaction);
                    }

                    foreach (var expenseItem in expenses)
                    {
                        var reportExpense = new ReportExpense
                        {
                            Report = report,
                            Expense = expenseItem
                        };
                        session.Save(reportExpense);
                    }

                    transaction.Commit();
                    return report;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteReport(int reportId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.CreateQuery("delete from ReportTransaction where Report.Id = :reportId")
                           .SetParameter("reportId", reportId)
                           .ExecuteUpdate();
                    session.CreateQuery("delete from ReportExpense where Report.Id = :reportId")
                           .SetParameter("reportId", reportId)
                           .ExecuteUpdate();

                    var report = session.Get<Report>(reportId);
                    if (report != null)
                    {
                        session.Delete(report);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

    }
}
