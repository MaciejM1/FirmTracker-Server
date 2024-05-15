using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Products;
using NHibernate;
using System.Collections.Generic;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;

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

        public decimal GetProductPrice(int productId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var product = session.Query<Product>()
                                     .Where(p => p.Id == productId)
                                     .Select(p => p.Price)
                                     .FirstOrDefault();

                return product;
            }
        }
        public int GetProductType(int productId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var product = session.Query<Product>()
                                     .Where(p => p.Id == productId)
                                     .Select(p => p.Type)
                                     .FirstOrDefault();

                return product;
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
