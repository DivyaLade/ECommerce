using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly EcommerceDbDevContext _dbContext;

        public CustomerController(EcommerceDbDevContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IEnumerable<Customer> GetCustomer()
        {
            return _dbContext.Customers;
        }
        [HttpGet("{id}")]
        public IEnumerable<Customer> Get(int id)
        {
            return _dbContext.Customers.Where(obj => obj.CustomerId == id).ToList();
        }
        [HttpPost("PostCustomer")]
        public ActionResult<Customer> PostCustomer([FromBody] Customer customer)
        {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPut("(Id)")]
        public async Task<ActionResult> UpdateCustomer(int id, Customer updateCustomer)

        {
            if (id != updateCustomer.CustomerId)
                return BadRequest("Customer id mismatch");

            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer Not Found");

            //Update entire Customer Object
            customer.Name = updateCustomer.Name;
            customer.Address = updateCustomer.Address;
            customer.Phone = updateCustomer.Phone;
            customer.Email = updateCustomer.Email;
            


            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("(Id)")]
        public async Task<ActionResult> PartialUpdateCustomer(int id, [FromBody] JsonPatchDocument<Customer> patchDoc)

        {
            if (patchDoc == null)
                return BadRequest("patch document can not be null");

            //Finf the Customer by id
            var Customer = await _dbContext.Customers.FindAsync(id);
            if (Customer == null)
                return NotFound("Customer Not Found");
            //Apply to 
            patchDoc.ApplyTo(Customer);

            await _dbContext.SaveChangesAsync();




            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var customer = _dbContext.Customers.FirstOrDefault(obj => obj.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            _dbContext.Customers.Remove(customer);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}

    

