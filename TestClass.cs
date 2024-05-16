using FirmTracker_Server.Controllers;
using FirmTracker_Server.nHibernate; 
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate.Transactions;
using FirmTracker_Server.nHibernate.Expenses;
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
            var product3 = new nHibernate.Products.Product
            {
                Name = "Produkt 2",
                Description = "produkt",
                Price = 16.50m,
                Type = 1,
                Availability = 20
            };
            var transaction1 = new Transaction
            {
                Date = DateTime.Now,
                Description = "testowa transakcja",
                Discount = 10,
                EmployeeId = 1,
                PaymentType = "Karta kredytowa",
            };
            var transaction2 = new Transaction
            {
                Date = DateTime.Now,
                Description = "testowa transakcja",
                Discount = 30,
                EmployeeId = 2,
                PaymentType = "Gotówka",
            };

            var expense1 = new Expense
            {
                Date = DateTime.Now,
                Value = 1003.9m,
                Description = "testowy rozchód"
            };

            try
            {
                FirmTracker_Server.nHibernate.Products.ProductCRUD productCrud = new ProductCRUD();
                FirmTracker_Server.nHibernate.Transactions.TransactionCRUD transactionCrud = new nHibernate.Transactions.TransactionCRUD();
                ExpenseCRUD expenseCrud = new ExpenseCRUD();
                productCrud.AddProduct(product);
                productCrud.AddProduct(product2);
                productCrud.AddProduct(product3);
                transactionCrud.AddTransaction(transaction1);
                transactionCrud.AddTransaction(transaction2);
                expenseCrud.AddExpense(expense1);
                

                List<TransactionProduct> testTransactionProducts = new List<TransactionProduct>  {
         new TransactionProduct { ProductID = 1, Quantity = 2 },
         new TransactionProduct { ProductID = 2, Quantity = 1 },
         new TransactionProduct { ProductID = 3, Quantity = 10 }
     };
                foreach (var transactionProduct in testTransactionProducts)
                {
                    transactionCrud.AddTransactionProductToTransaction(transaction1.Id, transactionProduct);

                }

                List<TransactionProduct> testTransactionProducts2 = new List<TransactionProduct>
                {
                    new TransactionProduct { ProductID = 3, Quantity=4},
                    new TransactionProduct { ProductID = 1, Quantity=6}
                };
                foreach (var transactionProduct in testTransactionProducts2)
                {
                    transactionCrud.AddTransactionProductToTransaction(transaction2.Id, transactionProduct);

                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
