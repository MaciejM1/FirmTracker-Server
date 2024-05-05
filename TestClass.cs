using FirmTracker_Server.Controllers;
using FirmTracker_Server.nHibernate; 
using FirmTracker_Server.nHibernate.Products;
using NHibernate;

namespace FirmTracker_Server
{
    public class TestClass
    {
        public void AddTestProduct()
        {
           // SessionFactory.Init(ConnectionString);

            var product = new nHibernate.Products.Product
            {
                Name = "Produkt 1",
                Description = "testowy produkt",
                Price = 11.50m,
                Type = 1, 
                Availability = 5
            };
            var product2 = new nHibernate.Products.Product
            {
                Name = "Usluga 1",
                Description = "testowa usluga",
                Price = 1120.00m,
                Type = 0,
                Availability = 0
            };
            var transaction1 = new nHibernate.Transactions.Transaction
            {
                Date = DateTime.Now,
                Description = "testowa transakcja",
                Discount = 10,
                EmployeeId = 1,
                PaymentType = "Karta kredytowa",
                Products = new List<Product> { product, product2 }
            };

            try
            {
                FirmTracker_Server.nHibernate.Products.ProductCRUD crud = new ProductCRUD();
                FirmTracker_Server.nHibernate.Transactions.TransactionCRUD transactionCrud = new nHibernate.Transactions.TransactionCRUD();
                crud.AddProduct(product);
                crud.AddProduct(product2);
                transactionCrud.AddTransaction(transaction1);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
