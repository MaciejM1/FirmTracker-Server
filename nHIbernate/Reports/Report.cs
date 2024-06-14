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
using Newtonsoft.Json;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class Report
    {
        public virtual int Id { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual decimal TotalIncome { get; set; }
        public virtual decimal TotalExpenses { get; set; }
        public virtual decimal TotalBalance { get; set; }

        /*public virtual IList<ReportTransaction> ReportTransactions { get; set; } = new List<ReportTransaction>();
        public virtual IList<ReportExpense> ReportExpenses { get; set; } = new List<ReportExpense>();

       
        public Report()
        {
            ReportTransactions = new List<ReportTransaction>();
            ReportExpenses = new List<ReportExpense>();
        }*/


        public class DateRangeDto
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

        }   
    }
}
