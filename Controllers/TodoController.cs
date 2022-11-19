using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;
using MyTodo.Data;
using MyTodo.Services;
using MyTodo.Utils;
using MyTodo.ViewModels;

# pragma warning disable CS1591
namespace MyTodo.Controllers {
  [ApiController]
  [Route("v1")]
  public class TodoController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly TodoService _todoService;
    public TodoController(AppDbContext context)
    {
      _context = context;
      _todoService = new TodoService(_context);
    }
    
    /**
     * <summary>
     * Get all todos
     * </summary>
     * <response code="200"> Return a To-do list </response>
     */
    [HttpGet]
    [ProducesResponseType(typeof(List<Todo>), 200)]
    [Route("todos")]
    [Produces("application/json")]
    public List<Todo> Get()
    {
      return _todoService.List();
    }
    /**
     * <summary>
     *  Get specific todo item
     * </summary>
     * <response code="200">Return a todo item</response>
     * <response code="404">Return 404 error</response>
     */
    [HttpGet]
    [Route("todos/{id}")]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public ActionResult<Todo> GetById ([FromRoute] int id)
    {
      var todo = _todoService.GetById(id);
      return todo;
    }
    
    /**
     * <summary>
     *  Create a todo item
     * </summary>
     * <param name="modelTodo">Schema for create a Todo</param>
     * <response code="503">Return 503 error, this service was unavailable</response>
     * <response code="400">Return 400 error, something is wrong in request</response>
     * <response code="201">Return created status with created item</response>
     * <returns> Todo </returns>
     */
    [HttpPost] 
    [Route("todos")]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public ActionResult<Todo> CreateTodo(
      [FromBody] CreateTodoViewModel modelTodo
    ) {
      if(!ModelState.IsValid) {
        return BadRequest();
      }
      var todo = _todoService.create(modelTodo);

      return StatusCode(StatusCodes.Status201Created, todo);
    }
    /**
     * <summary>
     *   Edit specific item
     * </summary>
     * <param name="id">The item identification</param>
     * <param name="modelTodo"> The body schema </param>
     * <response code="200">Return status 200, the item has been edited</response>
     * <response code="400">Return 400 error, something is wrong in request</response>
     * <response code="404">Return 404 error, item not found</response>
     * <response code="503">Return 503 error, this service was unavailable</response>
     */
    [HttpPut]
    [Route("todos/{id}")]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status503ServiceUnavailable)]
    [Produces("application/json")]
    public ActionResult<Todo> PutTodo(
      [FromRoute] int id,
      [FromBody] CreateTodoViewModel modelTodo
    ) {
      if (!ModelState.IsValid)
      {
        throw new AppError(HttpStatusCode.BadRequest, "Invalid title field");
      }
      
      var newTodo = _todoService.EditTodo(id, modelTodo);

      return StatusCode(StatusCodes.Status200OK, newTodo);
    }
    /**
     * <summary>
     *  Delete specific item
     * </summary>
     * <param name="id">The item identification</param>
     * <response code="200">Return status 200, the item has been deleted</response>
     * <response code="404">Return 404 error, item not found</response>
     * <response code="503">Return 503 error, this service was unavailable</response>
     */
    [HttpDelete]
    [Route("todos/{id}")]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IAppError), StatusCodes.Status503ServiceUnavailable)]
    [Produces("application/json")]
    public ActionResult<Todo> DeleteTodo(
      [FromRoute] int id
    )
    {
      var todo = _todoService.DeleteTodo(id);
      
      return StatusCode(StatusCodes.Status202Accepted, todo);
    }
  }
}