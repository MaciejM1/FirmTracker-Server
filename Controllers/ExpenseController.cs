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
