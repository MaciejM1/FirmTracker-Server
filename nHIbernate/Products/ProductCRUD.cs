using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Products;
using NHibernate;
using System.Collections.Generic;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FirmTracker_Server.nHibernate.Products
{
    public class ProductCRUD
    {
        public void AddProduct(Product product)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(product);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Product GetProduct(int productId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<Product>(productId);
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Update(product);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteProduct(int productId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var product = session.Get<Product>(productId);
                    if (product != null)
                    {
                        session.Delete(product);
                        transaction.Commit();
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IList<Product> GetAllProducts()
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<Product>().ToList();
            }
        }
    }

}
