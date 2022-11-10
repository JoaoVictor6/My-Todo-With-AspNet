using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;
using MyTodo.Data;
using MyTodo.ViewModels;

namespace MyTodo.Controllers {
  [ApiController]
  [Route("v1")]
  public class TodoController : ControllerBase{
    /**
     * <summary>
     * Get all todos
     * </summary>
     * <response code="200"> Return a To-do list </response>
     */
    [HttpGet]
    [ProducesResponseType(typeof(List<Todo>), 200)]
    [Route("todos")]
    [Produces("text/json")]
    public List<Todo> Get( [FromServices] AppDbContext context) {
      var todos = context
        .Todos
        .ToList();
      return todos;
    }
    /**
     * <summary>
     *  Get specific todo item
     * </summary>
     * <response code="200">Return a todo item</response>
     * <response code="400">Return 404 error</response>
     */
    [HttpGet]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("text/json")]
    public ActionResult<Todo> GetById (
      [FromServices] AppDbContext context, 
      [FromRoute] int id
    ) {
      var todo = context
        .Todos
        .FirstOrDefault(todo => todo.Id == id);

      if(todo == null) {
        return NotFound(); 
      };
      return todo;
    }
    
    /**
     * <summary>
     *  Create a todo item
     * </summary>
     * <response code="503">Return 503 error, this service was unavailable</response>
     * <response code="400">Return 400 error, something is wrong in request</response>
     * <response code="201">Return created status with created item</response>
     */
    [HttpPost]
    [Route("todos")]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("text/json")]
    public ActionResult<Todo> CreateTodo(
      [FromServices] AppDbContext context, 
      [FromBody] CreateTodoViewModel modelTodo
    ) {
      if(!ModelState.IsValid) {
        return BadRequest();
      }

      var todo = new Todo {
        Done = false,
        Title = modelTodo.Title,
      };

      try {
        context.Todos.Add(todo);
        context.SaveChanges();

        return Created($"v1/todos/{todo.Id}", todo);
      } catch {
        return StatusCode(503);
      }
    }
    /**
     * <summary>
     *   Edit specific item
     * </summary>
     * <param name="id">The item identification</param>
     * <response code="202">Return status 202, the item has been edited</response>
     * <response code="400">Return 400 error, something is wrong in request</response>
     * <response code="404">Return 404 error, item not found</response>
     * <response code="503">Return 503 error, this service was unavailable</response>
     */
    [HttpPut]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [Produces("text/json")]
    public ActionResult<Todo> PutTodo(
      [FromRoute] int id,
      [FromServices] AppDbContext context,
      [FromBody] CreateTodoViewModel modelTodo
    ) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      try {
        var todo = context
          .Todos
          .FirstOrDefault(todo => todo.Id == id);

        if(todo == null) {
          return NotFound();
        }

        todo.Title = modelTodo.Title;

        context.Todos.Update(todo);
        context.SaveChanges();
      
        return todo;
      } catch {
        return StatusCode(503);
      }

    }
    /**
     * <summary>
     *  Delete specific item
     * </summary>
     * <param name="id">The item identification</param>
     * <response code="202">Return status 202, the item has been deleted</response>
     * <response code="404">Return 404 error, item not found</response>
     * <response code="503">Return 503 error, this service was unavailable</response>
     */
    [HttpDelete]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [Produces("text/json")]
    public ActionResult<Todo> DeleteTodo(
      [FromRoute] int id,
      [FromServices] AppDbContext context
    ) {

      try {
        var todo = context
          .Todos
          .FirstOrDefault(todo => todo.Id == id);
        
        if(todo == null) {
          return NotFound();
        }

        context.Remove<Todo>(todo);
        context.SaveChanges();

        return StatusCode(202);
      } catch {
        return StatusCode(503);
      }
    }
  }
}