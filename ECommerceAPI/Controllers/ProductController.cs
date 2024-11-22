
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcommerceDbDevContext _dbContext;

        public ProductController(EcommerceDbDevContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Products;
        }
        [HttpGet("{id}")]
        public IEnumerable<Product> Get(int id)
        {
            return _dbContext.Products.Where(obj => obj.ProductId == id).ToList();
        }
        [HttpPost("PutProduct")]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPut("(Id)")]
        public async Task<ActionResult> UpdateProduct(int id, Product updateProduct)

        {
            if (id != updateProduct.ProductId)
                return BadRequest("product id mismatch");

            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product Not Found");

            //Update entire product Onject
            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Category = updateProduct.Category;
            product.Price = updateProduct.Price;
            product.Stock = updateProduct.Stock;


            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("(Id)")]
        public async Task<ActionResult> PartialUpdateProduct(int id,[FromBody] JsonPatchDocument<Product> patchDoc)

        {
            if (patchDoc == null)
                return BadRequest("patch document can not be null");

            //Finf the product by id
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product Not Found");
            //Apply to 
            patchDoc.ApplyTo(product);

            await _dbContext.SaveChangesAsync();

            


            return NoContent();
        }




        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(obj => obj.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return Ok();
        }
        public void Getdata()
        {
            //To do list;
        }

    }
}
