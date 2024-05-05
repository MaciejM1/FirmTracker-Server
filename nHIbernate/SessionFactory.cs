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
                    .AddFromAssemblyOf<Transactions.TransactionMapping>();           
                })
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))  //SchemaUpdate . Execute  dla only update
                .BuildSessionFactory();
        }
    }
}
