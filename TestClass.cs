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
                CreateProduct("Tarta_truskawka", "produkt", 31.99m, 1, 10),
                CreateProduct("Tarta_czekolada", "produkt", 30.99m, 1, 10),
                CreateProduct("Tarta_agrest", "produkt", 32.90m, 1, 8),
                CreateProduct("Tarta_pistacja", "produkt", 35.99m, 1, 12),
                CreateProduct("Tarta_karmel", "produkt", 32.00m, 1, 12),
                CreateProduct("Rolada_beza", "produkt", 21.00m, 1, 5),
                CreateProduct("Rolada_róża", "produkt", 21.90m, 1, 10),
                CreateProduct("Kostka_truskawka", "produkt", 12.00m, 1, 11),
                CreateProduct("Kostka_lemonCurd", "produkt", 13.99m, 1, 13),
                CreateProduct("Kostka_hiszpańska", "produkt", 11.99m, 1, 8),
                CreateProduct("Kostka_wiosenna", "produkt", 11.99m, 1, 5),
                CreateProduct("Kostka_jabłka", "produkt", 12.00m, 1, 5),
                CreateProduct("Kostka_porzeczka", "produkt", 12.99m, 1, 5),
                CreateProduct("Kostka_królewska", "produkt", 13.50m, 1, 5),
                CreateProduct("Kostka_czekolada", "produkt", 14.50m, 1, 10),
                CreateProduct("Kostka_wiśnia", "produkt", 12.50m, 1, 5),
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

            var transaction1 = new Transaction
            {
                Date = DateTime.Now.AddDays(-2),
                Description = "zamówienie telefon",
                Discount = 5,
                EmployeeId = 1,
                PaymentType = "Karta kredytowa",
            };
            var transaction2 = new Transaction
            {
                Date = DateTime.Now.AddDays(-3),
                Description = "sprzedaż - kasa",
                Discount = 30,
                EmployeeId = 2,
                PaymentType = "Gotówka",
            };
            var transaction3 = new Transaction
            {
                Date = DateTime.Now,
                Description = "sprzedaż - kasa",
                Discount = 15,
                EmployeeId = 1,
                PaymentType = "BLIK",
            };
            var transaction4 = new Transaction
            {
                Date = DateTime.Now,
                Description = "zamówienie",
                Discount = 15,
                EmployeeId = 1,
                PaymentType = "BLIK",
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

                    string queryUser = "insert into Users(Email,PassHash,Role) select '123@wp.pl', 'GOsGemJarMJu8btZKF6Rung27JLZkdO7Wfd4CwLhL1k=','User'";
                    string queryAdmin = "insert into Users(Email,PassHash,Role) select '321@wp.pl', 'GOsGemJarMJu8btZKF6Rung27JLZkdO7Wfd4CwLhL1k=','Admin'";

                   
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
                transactionCrud.AddTransaction(transaction1);
                transactionCrud.AddTransaction(transaction2);
                transactionCrud.AddTransaction(transaction3);
                transactionCrud.AddTransaction(transaction4);
                expenseCrud.AddExpense(expense1);
                expenseCrud.AddExpense(expense2);
                expenseCrud.AddExpense(expense3);

                List<TransactionProduct> testTransactionProducts = new List<TransactionProduct>  {
                    new TransactionProduct { ProductID =17, Quantity = 10 },
                    new TransactionProduct { ProductID = 14, Quantity = 1 },
                    new TransactionProduct { ProductID = 1, Quantity = 0 },
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
