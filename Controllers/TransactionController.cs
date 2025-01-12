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

using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.nHibernate.Transactions;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Transactions;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using FirmTracker_Server.Services;
using System.Security.Claims;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionCRUD _transactionCRUD;
        private readonly ProductCRUD _productCRUD;
        public TransactionController()
        {
            _transactionCRUD = new TransactionCRUD();
            _productCRUD = new ProductCRUD();
           
        }
        

        // POST: api/Transaction
        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult CreateTransaction([FromBody] nHibernate.Transactions.Transaction transaction)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                transaction.EmployeeId = int.Parse(userId);
                foreach (var product in transaction.TransactionProducts)
                {
                    // Validate if the product quantity is positive
                    if (product.Quantity <= 0)
                    {
                        return BadRequest($"Ilość na produktu {product.ProductName} musi być dodatnia.");
                    }
                    var productByName = _productCRUD.GetProductByName(product.ProductName);
                    if (productByName == null)
                    {
                        throw new InvalidOperationException($"Produkt o nazwie {product.ProductName} nie istnieje.");
                    }
                  
                   
                    product.ProductID = productByName.Id;
                    product.TransactionId = transaction.Id;

                    decimal price = productByName.Price;// _productCRUD.GetProductPrice(product.ProductID);
                    int type = productByName.Type;//_productCRUD.GetProductType(product.ProductID);
                    if (type == 1)
                    {
                        var prod = _productCRUD.GetProduct(product.ProductID);

                        if (product.Quantity > prod.Availability)
                        {
                            throw new InvalidOperationException($"Nie można dodać {prod.Name} do transakcji. Dostępność: {prod.Availability}, Zażądano: {product.Quantity}");
                            //return BadRequest($"Can't add product {product.ProductID} to transaction. Available: {availability}, Desired: {product.Quantity}");
                        }
                    }
                }

                _transactionCRUD.AddTransaction(transaction);

                
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; 
                    _transactionCRUD.UpdateTransactionProduct(product);
                }
                

              //  session.Flush(); 

                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (InvalidOperationException ioe)
            { 
                return BadRequest(ioe.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Transaction/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult GetTransaction(int id)
        {
            var transaction = _transactionCRUD.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult UpdateTransaction(int id, [FromBody] nHibernate.Transactions.Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest("ID transakcji nie zgadza się.");

            try
            {
                foreach (var product in transaction.TransactionProducts)
                {
                    // Validate if the product quantity is positive
                    if (product.Quantity <= 0)
                    {
                        return BadRequest($"Sprzedawana ilość produktu {product.ProductName} musi być ilością dodatnią.");
                    }
                    var productByName = _productCRUD.GetProductByName(product.ProductName);
                    if (productByName == null)
                    {
                        throw new InvalidOperationException($"Produkt o nazwie {product.ProductName} nie istnieje.");
                    }
                    product.ProductID = productByName.Id;
                    product.TransactionId = transaction.Id;

                    decimal price = productByName.Price;// _productCRUD.GetProductPrice(product.ProductID);
                    int type = productByName.Type;//_productCRUD.GetProductType(product.ProductID);
                }


                _transactionCRUD.UpdateTransaction(transaction);

               
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; 
                    _transactionCRUD.UpdateTransactionProduct(product);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult DeleteTransaction(int id)
        {
            try
            {
                _transactionCRUD.DeleteTransaction(id);
                return NoContent();
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest($"{ioe.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Transaction
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult GetAllTransactions()
        {
            var transactions = _transactionCRUD.GetAllTransactions();
            if (transactions == null)
                return NotFound();

            
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve 
            };

           // var json = JsonSerializer.Serialize(transactions, options);

            
            return Ok(transactions);
        }

        // DELETE: api/Transaction/5/product/10
        [HttpDelete("{transactionId}/product/{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        public IActionResult DeleteTransactionProduct(int transactionId, int productId)
        {
            try
            {
                _transactionCRUD.DeleteTransactionProduct(transactionId, productId);
                return NoContent();  
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest(ioe.Message);  
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);  
            }
        }
    }
}
