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

using FirmTracker_Server.Controllers;
using FirmTracker_Server.nHibernate; 
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate.Transactions;
using FirmTracker_Server.nHibernate.Expenses;
using NHibernate;
using FirmTracker_Server.Entities;
using FirmTracker_Server.Services;
using AutoMapper;
using FirmTracker_Server.Authentication;
using Microsoft.AspNetCore.Identity;
using FirmTracker_Server.Models;
using System.Data.SqlClient;

namespace FirmTracker_Server
{
    public class TestClass
    {
        public static Product CreateProduct(string name, string description, decimal price, int type, int availability)
        {
            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Type = type,
                Availability = availability
            };
        }
        public void AddTestProduct()
        {
            // SessionFactory.Init(ConnectionString);


           

            var products = new List<Product>
            {
                CreateProduct("Bon Prezentowy 100","bon o zadanej wartości 100 zł",100,0,1),
                CreateProduct("Bon Prezentowy 200","bon o zadanej wartości 200 zł",200,0,1),
                CreateProduct("Bon Prezentowy 300","bon o zadanej wartości 300 zł",300,0,1),
                CreateProduct("Bon Prezentowy 500","bon o zadanej wartości 500 zł",500,0,1),
                CreateProduct("Bon Prezentowy 1000", "bon o zadanej wartości 1000 zł", 1000, 0, 1),
                CreateProduct("Bon Prezentowy 2500", "bon o zadanej wartości 2500 zł", 2500, 0, 1),
                CreateProduct("Henna brwi i rzęs + regulacja", "usługa", 50, 0, 1),
                CreateProduct("Henna + regulacja", "usługa", 30, 0, 1),
                CreateProduct("Henna rzęs", "usługa", 20, 0, 1),
                CreateProduct("Regulacja", "usługa", 20, 0, 1),
                CreateProduct("Peeling + ampułka + maska algowa", "usługa", 100, 0, 1),
                CreateProduct("Pełne oczyszczanie twarzy z ultradźwiękami, mikrodermabrazją i algą", "usługa", 150, 0, 1),
                CreateProduct("Kawitacyjne oczyszczanie twarzy z ampułką i algą", "usługa", 150, 0, 1),
                CreateProduct("Oxybrazja tlenowa", "usługa", 150, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - wąsik", "usługa", 15, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - wąsik + broda", "usługa", 20, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - nogi do kolan", "usługa", 50, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - całe nogi", "usługa", 80, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - bikini", "usługa", 50, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - pachy", "usługa", 40, 0, 1),
                CreateProduct("Depilacja ciepłym woskiem - rąk", "usługa", 50, 0, 1),
                CreateProduct("Manicure z malowaniem lakierem", "usługa", 50, 0, 1),
                CreateProduct("Manicure z hybrydą", "usługa", 80, 0, 1),
                CreateProduct("Parafina na dłonie", "usługa", 40, 0, 1),
                CreateProduct("Manicure z parafiną", "usługa", 70, 0, 1),
                CreateProduct("Pedicure z pomalowaniem lakierem", "usługa", 100, 0, 1),
                CreateProduct("Pedicure z hybrydą", "usługa", 120, 0, 1),
                CreateProduct("Pedicure medyczny", "usługa", 100, 0, 1),
                CreateProduct("Założenie klamry tytanowej na wrastający paznokieć 1 szt.", "usługa", 120, 0, 1),
                CreateProduct("Usunięcie modzeli, odcisku", "usługa", 20, 50, 1),
                CreateProduct("Przekłucie uszu z kolczykami", "usługa", 50, 0, 1),
                CreateProduct("Doklejenie kępek 12 szt.", "usługa", 50, 0, 1),
                CreateProduct("Zabieg kwasem migdałowym, laktobionowym 40% pH 1.5", "usługa", 100, 0, 1),
                CreateProduct("Zabieg kwasem pirogronowym, azelainowym, salicylowym", "usługa", 100, 0, 1),
                CreateProduct("Peeling medyczny azelainowy, kwas ferulowy", "usługa", 150, 0, 1),
                CreateProduct("Usieciowany kwas hialuronowy medyczny 1 ml", "usługa", 700, 0, 1),
                CreateProduct("Nieusieciowany kwas hialuronowy (okolice oczu)", "usługa", 450, 0, 1),
                CreateProduct("Mezoterapia igłowa (koktajle witaminowe) twarzy i owłosionej skóry głowy", "usługa", 250, 0, 1),
                CreateProduct("Mezoterapia dermapenem twarz", "usługa", 250, 0, 1),
                CreateProduct("Mezoterapia igłowa dermapenem (twarz, szyja, dekolt)", "usługa", 400, 0, 1),
                CreateProduct("Makijaż okazjonalny", "usługa", 100, 0, 1),
                CreateProduct("Piercing klasyczny", "usługa", 75, 0, 1),
                 CreateProduct("Kolczyki do wygojenia - para", "produkt wykorzystywany", 10, 1, 48),


              };

           

            var expense1 = new Expense
            {
                Date = DateTime.Now,
                Value = 7999.9m,
                Description = "zakup maszyny do lodów FZ/2/6/2024"
            };
            var expense2 = new Expense
            {
                Date = DateTime.Parse("2024-09-10 16:11:17.6232408"),
                Value = 990.99m,
                Description = "naprawa pieca - 25.05.2024"
            };
            var expense3 = new Expense
            {
                Date = DateTime.Now,
                Value = 1800.00m,
                Description = "zakup składników "

            };

            try
            {
                string appDirectory = Directory.GetCurrentDirectory();
                string configFilePath = Path.Combine(appDirectory, "appsettings.json");
                string connectionString = "";
                if (File.Exists(configFilePath))
                {
                    var config = new ConfigurationBuilder()
                      .AddJsonFile(configFilePath)
                      .Build();

                    var connectionstringsection = config.GetSection("AppSettings:ConnectionString");

                    connectionString = connectionstringsection.Value;

                    //SessionFactory.Init(connectionString);

                    string queryAdmin = "insert into Users(Email,PassHash,Role) select 'julia.c03@wp.pl', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','Admin'";
                    string queryUser = "insert into Users(Email,PassHash,Role) select 'sylwia1972@gmail.com', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";
                    string queryUser2 = "insert into Users(Email,PassHash,Role) select '123@wp.pl', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";
                    string queryUser3 = "insert into Users(Email,PassHash,Role) select '321@wp.pl', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";
                    string queryUser4 = "insert into Users(Email,PassHash,Role) select 'magdalena.szwarc75@wp.pl', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";
                    string queryUser5 = "insert into Users(Email,PassHash,Role) select 'jac.ziel@gmail.com', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";
                    string queryUser6 = "insert into Users(Email,PassHash,Role) select 'renata.zielonka@wp.com', 'AQAAAAIAAYagAAAAEMQUuFPUNAddMmuZpCUAZpaDR31+BqMJhnamIAllDi+aTBJQ7tEtLuEMppgz0oLYyw==','User'";

                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryUser, connection);
                    command.CommandTimeout = 200;
                    command.ExecuteNonQuery();
                    connection.Close();

                 
                    SqlConnection connection2 = new SqlConnection(connectionString);
                    connection2.Open();

                    SqlCommand command2 = new SqlCommand(queryAdmin, connection2);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection3 = new SqlConnection(connectionString);
                    connection3.Open();

                    SqlCommand command3 = new SqlCommand(queryUser2, connection3);
                    command3.CommandTimeout = 200;
                    command3.ExecuteNonQuery();
                    connection3.Close();
                    SqlConnection connection4 = new SqlConnection(connectionString);
                    connection4.Open();

                    SqlCommand command4 = new SqlCommand(queryUser3, connection4);
                    command4.CommandTimeout = 200;
                    command4.ExecuteNonQuery();
                    connection4.Close();

                    SqlConnection connection5 = new SqlConnection(connectionString);
                    connection5.Open();

                    SqlCommand command5 = new SqlCommand(queryUser4, connection5);
                    command5.CommandTimeout = 200;
                    command5.ExecuteNonQuery();
                    connection5.Close();

                    SqlConnection connection6 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command6 = new SqlCommand(queryUser6, connection);
                    command6.CommandTimeout = 200;
                    command6.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection7 = new SqlConnection(connectionString);
                    connection7.Open();

                    SqlCommand command7 = new SqlCommand(queryUser5, connection7);
                    command7.CommandTimeout = 200;
                    command7.ExecuteNonQuery();
                    connection7.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Nie udało się dodać kont użytkowników " + e.Message);
            }



            try
            {
                FirmTracker_Server.nHibernate.Products.ProductCRUD productCrud = new ProductCRUD();
                FirmTracker_Server.nHibernate.Transactions.TransactionCRUD transactionCrud = new nHibernate.Transactions.TransactionCRUD();
               
                ExpenseCRUD expenseCrud = new ExpenseCRUD();
               // productCrud.AddProduct(product);
               
               // productCrud.AddProduct(product3);
                foreach(var clientProduct in products)
                {
                    productCrud.AddProduct(clientProduct);
                }
                /*transactionCrud.AddTransaction(transaction1);
                transactionCrud.AddTransaction(transaction2);
                transactionCrud.AddTransaction(transaction3);
                transactionCrud.AddTransaction(transaction4);
                transactionCrud.AddTransaction(transaction5);
                transactionCrud.AddTransaction(transaction6);
                transactionCrud.AddTransaction(transaction7);
                transactionCrud.AddTransaction(transaction8);
                transactionCrud.AddTransaction(transaction9);
                transactionCrud.AddTransaction(transaction10);
                transactionCrud.AddTransaction(transaction11);
                transactionCrud.AddTransaction(transaction12);
                transactionCrud.AddTransaction(transaction13);
                transactionCrud.AddTransaction(transaction14);
                transactionCrud.AddTransaction(transaction15);
                transactionCrud.AddTransaction(transaction16);
                transactionCrud.AddTransaction(transaction17);
                transactionCrud.AddTransaction(transaction18);
                transactionCrud.AddTransaction(transaction19);
                transactionCrud.AddTransaction(transaction20);*/
                expenseCrud.AddExpense(expense1);
                expenseCrud.AddExpense(expense2);
                expenseCrud.AddExpense(expense3);

                var transactions = new List<Transaction>
{
    new Transaction
    {
        Date = DateTime.Now.AddDays(-1),
        Description = "zamówienie",
        Discount = 5,
        EmployeeId = 1,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-2),
        Description = "sprzedaż",
        Discount = 10,
        EmployeeId = 2,
        PaymentType = "Karta kredytowa"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-3),
        Description = "sprzedaż",
        Discount = 15,
        EmployeeId = 3,
        PaymentType = "BLIK"
    },
    new Transaction
    {
        Date = DateTime.Now,
        Description = "sprzedaż",
        Discount = 20,
        EmployeeId = 4,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-5),
        Description = "sprzedaż",
        Discount = 8,
        EmployeeId = 1,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-6),
        Description = "na telefon",
        Discount = 12,
        EmployeeId = 2,
        PaymentType = "Karta kredytowa"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-7),
        Description = "sprzedaż",
        Discount = 18,
        EmployeeId = 3,
        PaymentType = "BLIK"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-8),
        Description = "rezerwacja",
        Discount = 25,
        EmployeeId = 4,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-10),
        Description = "sprzedaż",
        Discount = 9,
        EmployeeId = 1,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-11),
        Description = "sprzedaż",
        Discount = 14,
        EmployeeId = 2,
        PaymentType = "Karta kredytowa"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-12),
        Description = "sprzedaż w punkcie",
        Discount = 17,
        EmployeeId = 3,
        PaymentType = "BLIK"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-13),
        Description = "zamówienie",
        Discount = 22,
        EmployeeId = 4,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-15),
        Description = "sprzedaż",
        Discount = 7,
        EmployeeId = 1,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-16),
        Description = "sprzedaż",
        Discount = 13,
        EmployeeId = 2,
        PaymentType = "Karta kredytowa"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-17),
        Description = "sprzedaż",
        Discount = 16,
        EmployeeId = 3,
        PaymentType = "BLIK"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-18),
        Description = "sprzedaż",
        Discount = 21,
        EmployeeId = 4,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-20),
        Description = "sprzedaż",
        Discount = 10,
        EmployeeId = 1,
        PaymentType = "Gotówka"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-21),
        Description = "sprzedaż",
        Discount = 12,
        EmployeeId = 2,
        PaymentType = "Karta kredytowa"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-22),
        Description = "sprzedaż w punkcie",
        Discount = 14,
        EmployeeId = 3,
        PaymentType = "BLIK"
    },
    new Transaction
    {
        Date = DateTime.Now.AddDays(-23),
        Description = "sprzedaż",
        Discount = 18,
        EmployeeId = 4,
        PaymentType = "Gotówka"
    }
};


                var transactionProducts = new List<(int TransactionIndex, int ProductID, int Quantity)>
{
    (0, 1, 1), // Transaction 1: Product 1 with quantity 1
    (0, 2, 3), // Transaction 1: Product 2 with quantity 3
    (1, 3, 4), // Transaction 2: Product 3 with quantity 4
    (1, 4, 2), // Transaction 2: Product 4 with quantity 2
    (2, 5, 3), // Transaction 3: Product 5 with quantity 3
    (2, 6, 1), // Transaction 3: Product 6 with quantity 1
    (3, 7, 5), // Transaction 4: Product 7 with quantity 5
    (3, 8, 2), // Transaction 4: Product 8 with quantity 2
    (4, 9, 3), // Transaction 5: Product 9 with quantity 3
    (4, 10, 2), // Transaction 5: Product 10 with quantity 2
    (5, 11, 4), // Transaction 6: Product 11 with quantity 4
    (5, 12, 1), // Transaction 6: Product 12 with quantity 1
    (6, 13, 3), // Transaction 7: Product 13 with quantity 3
    (6, 14, 2), // Transaction 7: Product 14 with quantity 2
    (7, 15, 5), // Transaction 8: Product 15 with quantity 5
    (7, 16, 2), // Transaction 8: Product 16 with quantity 2
    (8, 17, 3), // Transaction 9: Product 17 with quantity 3
    (8, 18, 4), // Transaction 9: Product 18 with quantity 4
    (9, 19, 2), // Transaction 10: Product 19 with quantity 2
    (9, 20, 3), // Transaction 10: Product 20 with quantity 3
    (10, 1, 1), // Transaction 11: Product 1 with quantity 1
    (10, 2, 5), // Transaction 11: Product 2 with quantity 5
    (11, 3, 2), // Transaction 12: Product 3 with quantity 2
    (11, 4, 3), // Transaction 12: Product 4 with quantity 3
    (12, 5, 1), // Transaction 13: Product 5 with quantity 1
    (12, 6, 4), // Transaction 13: Product 6 with quantity 4
    (13, 7, 2), // Transaction 14: Product 7 with quantity 2
    (13, 8, 3), // Transaction 14: Product 8 with quantity 3
    (14, 9, 3), // Transaction 15: Product 9 with quantity 3
    (14, 10, 1), // Transaction 15: Product 10 with quantity 1
    (15, 11, 2), // Transaction 16: Product 11 with quantity 2
    (15, 12, 3), // Transaction 16: Product 12 with quantity 3
    (16, 13, 3), // Transaction 17: Product 13 with quantity 3
    (16, 14, 1), // Transaction 17: Product 14 with quantity 1
    (17, 15, 4), // Transaction 18: Product 15 with quantity 4
    (17, 16, 1), // Transaction 18: Product 16 with quantity 1
    (18, 17, 2), // Transaction 19: Product 17 with quantity 2
    (18, 18, 3), // Transaction 19: Product 18 with quantity 3
    (19, 19, 1), // Transaction 20: Product 19 with quantity 1
    (19, 20, 2), // Transaction 20: Product 20 with quantity 2
};


                // Add transactions
                foreach (var transaction in transactions)
                {
                    transactionCrud.AddTransaction(transaction);
                }

                // Add transaction products
                foreach (var transactionProduct in transactionProducts)
                {
                    var transactionId = transactions[transactionProduct.TransactionIndex].Id;
                    transactionCrud.AddTransactionProductToTransaction(
                        transactionId,
                        new TransactionProduct
                        {
                            ProductID = transactionProduct.ProductID,
                            Quantity = transactionProduct.Quantity
                        }
                    );
                }



                /*                List<TransactionProduct> testTransactionProducts = new List<TransactionProduct>  {
                                    new TransactionProduct { ProductID =17, Quantity = 3 },
                                    new TransactionProduct { ProductID = 14, Quantity = 1 },
                                    new TransactionProduct { ProductID = 1, Quantity = 1 },
                                };
                                foreach (var transactionProduct in testTransactionProducts)
                                {
                                    transactionCrud.AddTransactionProductToTransaction(transaction1.Id, transactionProduct);

                                }

                                List<TransactionProduct> testTransactionProducts2 = new List<TransactionProduct>
                                {
                                    new TransactionProduct { ProductID = 28, Quantity=5},
                                    new TransactionProduct { ProductID = 22, Quantity=5}
                                };
                                foreach (var transactionProduct in testTransactionProducts2)
                                {
                                    transactionCrud.AddTransactionProductToTransaction(transaction2.Id, transactionProduct);

                                }

                                List<TransactionProduct> testTransactionProducts3 = new List<TransactionProduct>
                                {
                                    new TransactionProduct { ProductID = 3, Quantity=9},
                                    new TransactionProduct { ProductID = 2, Quantity=1}
                                };
                                foreach (var transactionProduct in testTransactionProducts3)
                                {
                                    transactionCrud.AddTransactionProductToTransaction(transaction3.Id, transactionProduct);

                                }
                                List<TransactionProduct> testTransactionProducts4 = new List<TransactionProduct>
                                {
                                    new TransactionProduct { ProductID = 33, Quantity=12},
                                    new TransactionProduct { ProductID = 12, Quantity=1}
                                };
                                foreach (var transactionProduct in testTransactionProducts4)
                                {
                                    transactionCrud.AddTransactionProductToTransaction(transaction4.Id, transactionProduct);

                                }*/

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
