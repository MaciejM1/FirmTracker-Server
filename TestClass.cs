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


            var product2 = new nHibernate.Products.Product
            {
                Name = "Dostawa",
                Description = "usługa dostawy",
                Price = 7.50m,
                Type = 0,
                Availability = 0
            };
         
            var products = new List<Product>
            {
                CreateProduct("Tarta_truskawka", "produkt", 31.99m, 1, 20),
                CreateProduct("Tarta_czekolada", "produkt", 30.99m, 1, 20),
                CreateProduct("Tarta_agrest", "produkt", 32.90m, 1, 10),
                CreateProduct("Tarta_pistacja", "produkt", 35.99m, 1, 15),
                CreateProduct("Tarta_karmel", "produkt", 32.00m, 1, 15),
                CreateProduct("Rolada_beza", "produkt", 21.00m, 1, 12),
                CreateProduct("Rolada_róża", "produkt", 21.90m, 1, 10),
                CreateProduct("Kostka_truskawka", "produkt", 12.00m, 1, 15),
                CreateProduct("Kostka_lemonCurd", "produkt", 13.99m, 1, 15),
                CreateProduct("Kostka_hiszpańska", "produkt", 11.99m, 1, 10),
                CreateProduct("Kostka_wiosenna", "produkt", 11.99m, 1, 10),
                CreateProduct("Kostka_jabłka", "produkt", 12.00m, 1, 15),
                CreateProduct("Kostka_porzeczka", "produkt", 12.99m, 1, 10),
                CreateProduct("Kostka_królewska", "produkt", 13.50m, 1, 20),
                CreateProduct("Kostka_czekolada", "produkt", 14.50m, 1, 12),
                CreateProduct("Kostka_wiśnia", "produkt", 12.50m, 1, 10),
                CreateProduct("Kostka_beza", "produkt", 13.50m, 1, 20),
                CreateProduct("Kostka_leśna", "produkt", 12.00m, 1, 20),
                CreateProduct("Kostka_kawowa", "produkt", 12.00m, 1, 10),
                CreateProduct("Kostka_galaretka", "produkt", 12.50m, 1, 25),
                CreateProduct("Kostka_firmowa", "produkt", 12.50m, 1, 5),
                CreateProduct("Sernik_wiśnia", "produkt", 33.00m, 1, 6),
                CreateProduct("Sernik_truskawka", "produkt", 31.00m, 1, 5),
                CreateProduct("Sernik_pistacja", "produkt", 38.90m, 1, 5),
                CreateProduct("Sernik_fantazja", "produkt", 33.00m, 1, 7),
                CreateProduct("Sernik_rafaello", "produkt", 33.00m, 1, 5),
                CreateProduct("Sernik_nutella", "produkt", 35.50m, 1, 6),
                CreateProduct("Sernik_mango", "produkt", 33.00m, 1, 5),
                CreateProduct("Sernik_rabarbar", "produkt", 37.99m, 1, 5),
                CreateProduct("Sernik_biszkopt", "produkt", 39.00m, 1, 11),
                CreateProduct("Tartaletka", "produkt", 13.20m, 1, 30),
                CreateProduct("Strudel_jabłko", "produkt", 29.00m, 1, 20),
                CreateProduct("Placek_rabarbar", "produkt", 24.00m, 1, 18),
                CreateProduct("Placek_jogurt", "produkt", 23.00m, 1, 13),
                CreateProduct("Placek_śliwka", "produkt", 22.00m, 1, 14),
                CreateProduct("Placek_maślany", "produkt", 18.00m, 1,11),
                CreateProduct("Keks", "produkt", 22.00m, 1,11),
                CreateProduct("Babka_drożdżowa", "produkt", 16.00m, 1,11),
                CreateProduct("Pączek_pistacja", "produkt", 8.00m, 1,11),
                CreateProduct("Pączek_marmolada", "produkt", 3.00m, 1,11),
                CreateProduct("Pączek_nutella", "produkt", 4.50m, 1,11),
                CreateProduct("Pączek_rafaello", "produkt", 4.50m, 1,11),
                CreateProduct("Pączek_róża", "produkt", 4.00m, 1,11),
                CreateProduct("Ekler", "produkt", 3.00m, 1,11),
                CreateProduct("Ekler_słony_karmel", "produkt", 5.00m, 1,11),
                CreateProduct("Ptyś", "produkt", 4.00m, 1,11),
                CreateProduct("Drożdżówka_ser", "produkt", 4.00m, 1,11),
                CreateProduct("Drożdżówka_rabarbar", "produkt", 5.00m, 1,11),
                CreateProduct("Drożdżówka_żurawina", "produkt", 5.00m, 1,11),
                CreateProduct("Drożdżówka_kruszonka", "produkt", 4.00m, 1,11),
                CreateProduct("Drożdżówka_budyń", "produkt", 5.00m, 1,11),
                CreateProduct("Jagodzianka", "produkt", 6.00m, 1,11),


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
                    connection.Open();

                    SqlCommand command2 = new SqlCommand(queryAdmin, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection3 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command3 = new SqlCommand(queryUser2, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();
                    SqlConnection connection4 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command4 = new SqlCommand(queryUser3, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection5 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command5 = new SqlCommand(queryUser4, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection6 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command6 = new SqlCommand(queryUser6, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();

                    SqlConnection connection7 = new SqlConnection(connectionString);
                    connection.Open();

                    SqlCommand command7 = new SqlCommand(queryUser5, connection);
                    command2.CommandTimeout = 200;
                    command2.ExecuteNonQuery();
                    connection2.Close();
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
                productCrud.AddProduct(product2);
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
        Description = "zamówienie telefoniczne",
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
        Description = "zamówienie",
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
        Description = "na telefon",
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
        Description = "zamówienie telefoniczne",
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
        Description = "zamówienie online",
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
