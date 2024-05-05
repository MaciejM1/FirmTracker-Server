using Microsoft.AspNetCore.Mvc;
using FirmTracker_Server.nHibernate.Transactions;
using FirmTracker_Server;
using System.Collections.Generic;


namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionCRUD _transactionCRUD;

        public TransactionController()
        {
            _transactionCRUD = new TransactionCRUD();
        }

        // POST: api/Transaction
        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                _transactionCRUD.AddTransaction(transaction);
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
        public IActionResult UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest("Transaction ID mismatch");

            try
            {
                _transactionCRUD.UpdateTransaction(transaction);
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
        public IActionResult GetAllTransactions()
        {
            var transactions = _transactionCRUD.GetAllTransactions();
            return Ok(transactions);
        }

    }
}