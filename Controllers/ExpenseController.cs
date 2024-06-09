using FirmTracker_Server.nHibernate.Expenses;
using Microsoft.AspNetCore.Mvc;
namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseCRUD _expenseCrud;
        
        public ExpensesController()
        {
            _expenseCrud = new ExpenseCRUD();
        }
        // POST: api/Expenses
        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult CreateExpense([FromBody] Expense expense) { 
        try
            {
                _expenseCrud.AddExpense(expense);
                return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
            }
        catch (System.Exception ex) {
                return BadRequest(ex.Message);
                    }
        }

        // GET: api/Expenses
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(404)] // Bad Request
        public IActionResult GetExpense(int id)
        {
            var expense = _expenseCrud.GetExpense(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        //PUT: api/Expenses
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest("Expense ID mismatch");
            }
            try
            {
                _expenseCrud.UpdateExpense(expense);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteExpense(int id)
        {
            try
            {
                _expenseCrud.DeleteExpense(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllExpenses()
        {
            try
            {
                var expenses = _expenseCrud.GetAllExpenses();
                return Ok(expenses);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    
}
