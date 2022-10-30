using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;
using MyTodo.Data;
using MyTodo.ViewModels;

namespace MyTodo.Controllers {
  [ApiController]
  [Route("v1")]
  public class TodoController : ControllerBase{
    [HttpGet]
    [Route("todos")]
    public List<Todo> Get( [FromServices] AppDbContext context) {
      var todos = context
        .Todos
        .ToList();
      return todos;
    }

    [HttpGet]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpPost]
    [Route("todos")]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status201Created)]
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

    [HttpPut]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
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
  
  
    [HttpDelete]
    [Route("todos/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
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