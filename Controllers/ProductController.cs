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

using FirmTracker_Server.nHibernate.Products;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FirmTracker_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductCRUD _productCrud;

        public ProductsController()
        {
            _productCrud = new ProductCRUD();
        }

        // POST: api/Products
        /// <summary>
        /// Creates a new product.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product.Type != 0 && product.Type != 1)
            {
                throw new InvalidOperationException("Kategoria produktu musi być ustawiona na 0 lub 1.");
            }
            if (product.Type == 0 && product.Availability != 0)
            {
                throw new InvalidOperationException("Dostępność usługi musi być ustawiona na 0.");
            }
            if(product.Type ==1 && product.Availability < 0) {
                throw new InvalidOperationException("Dostępność towaru nie może być ujemna.");
            }
            if (product.Price < 0)
            {
                throw new InvalidOperationException("Produkt nie może posiadać ujemnej ceny.");
            }
            try
            {
                _productCrud.AddProduct(product);
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult GetProduct(int id)
        {
            var product = _productCrud.GetProduct(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetProductByName(string name)
        {
            var product = _productCrud.GetProductByName(name);
            if (product ==null)
                return NotFound();
            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                throw new InvalidOperationException("ID produktu nie zgadza się.");
            if (product.Type != 0 && product.Type != 1)
            {
                throw new InvalidOperationException("Kategoria produktu musi być ustawiona na 0 lub 1.");
            }
            if (product.Type == 0 && product.Availability != 0)
            {
                throw new InvalidOperationException("Dostępność usługi musi być ustawiona na 0.");
            }
            if (product.Type == 1 && product.Availability < 0)
            {
                throw new InvalidOperationException("Dostępność towaru nie może być ujemna.");
            }
            if (product.Price < 0)
            {
                throw new InvalidOperationException("Produkt nie może posiadać ujemnej ceny.");
            }
            try
            {
                _productCrud.UpdateProduct(product);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productCrud.DeleteProduct(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(200)] // Created
        [ProducesResponseType(400)] // Bad Request
        public IActionResult GetAllProducts()
        {
            var products = _productCrud.GetAllProducts();
            return Ok(products);
        }

        [HttpPost("CalculateTotalPrice")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CalculateTotalPrice([FromBody] ProductOrder[] orders)
        {
            decimal totalPrice = 0;
            decimal discount = 0;
            foreach (var order in orders)
            {
                discount = order.Discount;
                var product = _productCrud.GetProduct(order.ProductId);
                if (product == null)
                {
                    return BadRequest($"Nie znaleziono produktu o ID {order.ProductId}.");
                }
                totalPrice += product.Price * order.Quantity;
            }

            // Apply discount
            decimal discountAmount = totalPrice * (discount / 100);
            totalPrice -= discountAmount;

            return Ok(new { TotalPrice = totalPrice });
        }

        public class ProductOrder
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Discount { get; set; }   
        }
    }
}