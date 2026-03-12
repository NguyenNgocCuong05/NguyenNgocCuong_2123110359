using Microsoft.AspNetCore.Mvc;
using NguyenNgocCuong_2123110359.Models;

namespace NguyenNgocCuong_2123110359.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static readonly List<string> products = new List<string>
        {
            "Product 1",
            "Product 2",
            "Product 3"
        };

        private static readonly object _lock = new();

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            lock (_lock)
            {
                return Ok(products);
            }
        }

        // GET: api/Product/0
        [HttpGet("{id:int}", Name = "GetProductById")]
        public ActionResult<string> GetById(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= products.Count)
                    return NotFound();

                return Ok(products[id]);
            }
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult Create([FromBody] NameDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name required.");

            int newIndex;
            lock (_lock)
            {
                products.Add(dto.Name);
                newIndex = products.Count - 1;
            }

            return CreatedAtRoute("GetProductById", new { id = newIndex }, new { id = newIndex, name = dto.Name });
        }

        // PUT: api/Product/0
        [HttpPut("{id:int}")]
        public ActionResult Update(int id, [FromBody] NameDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name required.");

            lock (_lock)
            {
                if (id < 0 || id >= products.Count)
                    return NotFound();

                products[id] = dto.Name;
            }

            return NoContent();
        }

        // DELETE: api/Product/0
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= products.Count)
                    return NotFound();

                products.RemoveAt(id);
            }

            return NoContent();
        }
    }
}
