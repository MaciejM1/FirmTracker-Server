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

using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Products;
using NHibernate;
using System.Collections.Generic;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;
using FirmTracker_Server.nHibernate.Reports;
using NHibernate.Criterion;
using FirmTracker_Server.nHibernate.Transactions;

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

        public int GetProductAvailability(int productId)
        {
            using(var session = SessionFactory.OpenSession())
            {
                var product = session.Query<Product>()
                    .Where(p => p.Id == productId)
                    .Select(p => p.Availability)
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

        public Product GetProductByName(string productName)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("from Product where Name = :name");
                query.SetParameter("name", productName);
                return query.UniqueResult<Product>();
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

                        var criteria = session.CreateCriteria<TransactionProduct>();
                        criteria.Add(Restrictions.Eq("ProductID", productId));
                        var transactionProducts = criteria.List<TransactionProduct>();

                        if (transactionProducts.Any())
                        {
                            throw new InvalidOperationException("Nie można usunąć produktu. Produkt jest ujęty w co najmniej jednej transakcji.");
                        }

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
