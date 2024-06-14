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

using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportTransactionMapping : ClassMap<ReportTransaction>
    {
        public ReportTransactionMapping()
        {
            Table("ReportTransactions");
            CompositeId()
                .KeyReference(x => x.Report, "ReportId")
                .KeyReference(x => x.Transaction, "TransactionId");
        }
    }
}
