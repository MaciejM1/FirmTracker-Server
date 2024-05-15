using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.nHibernate.Transactions;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Transactions;
using FirmTracker_Server.nHibernate.Products;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult CreateTransaction([FromBody] nHibernate.Transactions.Transaction transaction)
        {
            try
            {
                // Before adding the transaction, ensure each product is linked properly
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; // This might be 0 at this point if transaction isn't saved yet
                    decimal price = _productCRUD.GetProductPrice(product.ProductID);
                    int type = _productCRUD.GetProductType(product.ProductID);
                    if (type == 1)
                    {
                        transaction.TotalPrice += ((product.Quantity * price) * ((1 - (transaction.Discount / 100))));
                    }
                    else
                    {
                        transaction.TotalPrice += (price * ((1 - (transaction.Discount / 100))));
                    }
                }

                _transactionCRUD.AddTransaction(transaction);

                // Now that the transaction is saved, update each product with the correct TransactionId
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; // Now transaction.Id is a valid ID after saving
                    _transactionCRUD.UpdateTransactionProduct(product);
                }
                

              //  session.Flush(); // Ensure changes are committed if managing session manually

                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
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
        public IActionResult UpdateTransaction(int id, [FromBody] nHibernate.Transactions.Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest("Transaction ID mismatch");

            try
            {
                // Before adding the transaction, ensure each product is linked properly
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; // This might be 0 at this point if transaction isn't saved yet
                    decimal price = _productCRUD.GetProductPrice(product.ProductID);
                    transaction.TotalPrice += ((product.Quantity * price) * ((1 - (transaction.Discount / 100))));
                }
                _transactionCRUD.UpdateTransaction(transaction);

                // Now that the transaction is saved, update each product with the correct TransactionId
                foreach (var product in transaction.TransactionProducts)
                {
                    product.TransactionId = transaction.Id; // Now transaction.Id is a valid ID after saving
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
        public IActionResult DeleteTransaction(int id)
        {
            try
            {
                _transactionCRUD.DeleteTransaction(id);
                return NoContent();
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
        public IActionResult GetAllTransactions()
        {
            var transactions = _transactionCRUD.GetAllTransactions();
            if (transactions == null)
                return NotFound();

            // Ustawienie opcji serializatora JSON
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve // Obsługa cykli obiektów
            };

           // var json = JsonSerializer.Serialize(transactions, options);

            // Zwrócenie odpowiedzi z JSON
            return Ok(transactions);
        }

    }
}
