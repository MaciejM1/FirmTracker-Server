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

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportExpense
    {
        //public virtual int Id { get; set; }
        public virtual Report Report { get; set; }
        public virtual Expense Expense { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ReportExpense)obj;
            return Report != null && Expense != null &&
                   Report.Id == other.Report.Id &&
                   Expense.Id == other.Expense.Id;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + (Report?.Id.GetHashCode() ?? 0);
                hash = hash * 23 + (Expense?.Id.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

}
