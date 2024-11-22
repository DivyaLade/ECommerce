using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        
            private readonly EcommerceDbDevContext _dbContext;

            public OrdersController(EcommerceDbDevContext dbContext)
            {
                _dbContext = dbContext;
            }
            [HttpGet]
            public IEnumerable<Order> GetProducts()
            {
                return _dbContext.Orders;
            }
            [HttpGet("{id}")]
            public IEnumerable<Order> Get(int id)
            {
                return _dbContext.Orders.Where(obj => obj.OrderId == id).ToList();
            }
            [HttpPost("Postorder")]
            public ActionResult<Order> PostOrder([FromBody] Order order)
            {
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                return Ok();
            }
        [HttpPut("(Id)")]
        public async Task<ActionResult> UpdateOeders(int id, Order updateOrders)

        {
            if (id != updateOrders.OrderId)
                return BadRequest("Order id mismatch");

            var Order = await _dbContext.Customers.FindAsync(id);
            if (Order == null)
                return NotFound("Order Not Found");

            //Update entire Order Object
            Order.CustomerId = id;
            


            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("(Id)")]
        public async Task<ActionResult> PartialUpdateOrder(int id, [FromBody] JsonPatchDocument<Order> patchDoc)

        {
            if (patchDoc == null)
                return BadRequest("patch document can not be null");

            //Finf the Order by id
            var Order = await _dbContext.Orders.FindAsync(id);
            if (Order == null)
                return NotFound("Order Not Found");
            //Apply to 
            patchDoc.ApplyTo(Order);

            await _dbContext.SaveChangesAsync();




            return NoContent();
        }
        [HttpDelete("{id}")]
            public ActionResult Delete(int id)
            {
                var Order = _dbContext.Orders.FirstOrDefault(obj => obj.OrderId == id);
                if (Order == null)
                {
                    return NotFound();
                }
                _dbContext.Orders.Remove(Order);
                _dbContext.SaveChanges();
                return Ok();
            }

        }
    }



