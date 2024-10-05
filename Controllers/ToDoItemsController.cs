using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly DataContext _context;

    public ToDoItemsController(DataContext context)
    {
        _context = context;
    }

    
     [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            return await _context.ToDoItems
                .Where(t => t.CompletedDate == null)
                .ToListAsync();
        }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem item)
    {
        _context.ToDoItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDoItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutToDoItem(int id, ToDoItem item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        item.CompletedDate = DateTime.UtcNow;

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ToDoItemExists(id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ToDoItemExists(int id)
    {
        return _context.ToDoItems.Any(e => e.Id == id);
    }
}
