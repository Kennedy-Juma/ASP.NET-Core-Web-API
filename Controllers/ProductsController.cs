using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasics.Data;
using WebApiBasics.Models;

namespace WebApiBasics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;

        }

        //Get a list of all the products
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var response=await _context.Product.ToListAsync();
            return Ok(response);

        }

        //Get product based on id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var Product = await _context.Product.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            return Ok(Product);
        }
        //Create new product
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
           _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);

        }
        /*
                //Update a product
                [HttpPut("{id}")]
                public async Task<ActionResult> PutProduct(int id, Product Product)
                {

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    _context.Entry(Product).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Product.Any(p => p.Id == id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return Ok(Product);//AcceptedAtAction("GetProduct", new { id = Product.Id, name = Product.Name }, Product);
                }
        */

        //Update a product
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            var productToUpdate = _context.Product.FirstOrDefault(e => e.Id == id);

            if (productToUpdate != null && product != null)
            {
                foreach (var property in typeof(Product).GetProperties())
                {
                    if (property.Name != "Id")
                    {
                        var newValue = property.GetValue(product);

                        if (newValue != null)
                        {
                            property.SetValue(productToUpdate, newValue);
                        }

                            
                    }
                
                }

              await  _context.SaveChangesAsync();
            }

            return Ok(productToUpdate);
        }



        //Delete a product
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        //Delete multiple products
        [HttpDelete]
        public async Task<ActionResult> DeleteProducts([FromQuery] int[] id)
        {
            if (id == null || id.Length == 0)
            {
                return BadRequest("No product IDs provided.");
            }

            var productsToDelete = _context.Product.Where(p => id.Contains(p.Id));
            _context.Product.RemoveRange(productsToDelete);
           await _context.SaveChangesAsync();

            return Ok(productsToDelete);
        }



    }
}
