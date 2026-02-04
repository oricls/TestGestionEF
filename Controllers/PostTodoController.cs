using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProjectApi.Entities;
using TestProjectApi.Models;

namespace TestProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItem()
        {
            return await _context.TodoItems.Select(x => MapToDTO(x)).ToListAsync();
        }       


        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return NotFound();
            return MapToDTO(item);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified; // ?

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {   
                if (!CheckExists(id))
                {
                   return NotFound(); 
                }
                else
                {
                  throw;
                }
                
            }
            return NoContent();        
        }
        
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO item)
        {
            var itemModel = MapToModel(item);

            _context.TodoItems.Add(itemModel);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, MapToDTO(itemModel));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem (int id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null) return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // mappage : (pas le bon endroit)
        private static TodoItemDTO MapToDTO(TodoItem item)
        {
            return new TodoItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                IsCompleted = item.IsCompleted
            };
        }

        private static TodoItem MapToModel(TodoItemDTO item)
        {
            return new TodoItem {
                IsCompleted = item.IsCompleted,
                Name = item.Name
            };
        }
        
        private bool CheckExists(int id) => _context.TodoItems.Any(e => e.Id == id);

    
    }
}
