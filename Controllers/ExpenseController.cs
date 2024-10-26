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
using FirmTracker_Server.nHibernate.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult CreateExpense([FromBody] Expense expense) {
            try
            {
                if (expense.Value <= 0)
                {
                    throw new InvalidOperationException("Wydatek nie może posiadać kwoty mniejszej lub równej 0.");
                }

                _expenseCrud.AddExpense(expense);
                return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
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

        // GET: api/Expenses
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(404)] // Bad Request
        [Authorize(Roles = Roles.Admin)]
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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpdateExpense(int id, [FromBody] Expense expense)
        {
            try
            {
                if (id != expense.Id)
                {
                    return BadRequest("Nieprawidłowe ID wydatku");
                }
                if (expense.Value <= 0)
                {
                    throw new InvalidOperationException("Wydatek nie może posiadać kwoty mniejszej lub równej 0.");
                }


                _expenseCrud.UpdateExpense(expense);
                return NoContent();
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

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteExpense(int id)
        {
            try
            {
                _expenseCrud.DeleteExpense(id);
                return NoContent();
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest($"{ioe.Message}");
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize(Roles = Roles.Admin)]
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
