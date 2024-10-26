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

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FirmTracker_Server.nHibernate
{
    public static class SessionFactory
    {
        private static ISessionFactory _factory;

        public static NHibernate.ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        public static void Init(string connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
        .ConnectionString(c => c.Is(connectionString))
        .ShowSql())
                .Mappings(m =>
                {
                    m.FluentMappings
                    .AddFromAssemblyOf<Products.ProductMapping>()
                    .AddFromAssemblyOf<Transactions.TransactionMapping>()
                    .AddFromAssemblyOf<Transactions.TransactionProductMapping>()
                    .AddFromAssemblyOf<Transactions.Transaction2Mapping>()
                    .AddFromAssemblyOf<Transactions.TransactionWithProductsMapping>()
                    .AddFromAssemblyOf<Expenses.ExpenseMapping>()
                    .AddFromAssemblyOf<Reports.ReportMapping>()
                    .AddFromAssemblyOf<Reports.ReportTransactionMapping>()
                    .AddFromAssemblyOf<Reports.ReportExpenseMapping>()
                    .AddFromAssemblyOf<LogsMapping>()
                    .AddFromAssemblyOf<UserMapping>();
                    
                })
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))  //SchemaUpdate . Execute  dla only update
                .BuildSessionFactory();
        }
    }
}
