using Microsoft.AspNetCore.Mvc;
using NguyenNgocCuong_2123110359.Models;

namespace NguyenNgocCuong_2123110359.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase            
    {
        private static readonly List<string> _categories = new List<string>
        {
            "value1",
            "value2"
        };

        private static readonly object _lock = new();

        // GET: api/Category
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            lock (_lock)
            {
                return Ok(_categories);
            }
        }

        // GET: api/Category/0
        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public ActionResult<string> GetById(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _categories.Count)
                    return NotFound();

                return Ok(_categories[id]);
            }
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult Create([FromBody] NameDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name required.");

            int newIndex;
            lock (_lock)
            {
                _categories.Add(dto.Name);
                newIndex = _categories.Count - 1;
            }

            return CreatedAtRoute("GetCategoryById", new { id = newIndex }, new { id = newIndex, name = dto.Name });
        }

        // PUT: api/Category/0
        [HttpPut("{id:int}")]
        public ActionResult Update(int id, [FromBody] NameDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name required.");

            lock (_lock)
            {
                if (id < 0 || id >= _categories.Count)
                    return NotFound();

                _categories[id] = dto.Name;
            }

            return NoContent();
        }

        // DELETE: api/Category/0
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _categories.Count)
                    return NotFound();

                _categories.RemoveAt(id);
            }

            return NoContent();
        }
    }
}
