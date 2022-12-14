using System.Net;
using MyTodo.Data;
using MyTodo.Models;
using MyTodo.Utils;
using MyTodo.ViewModels;
#pragma warning disable CS1591
namespace MyTodo.Services;

public class TodoService
{
  private AppDbContext _context;
  public TodoService(AppDbContext dbContext)
  {
    _context = dbContext;
  }
  
  private static void serviceUnavailable(string method)
  {
    throw new AppError(HttpStatusCode.ServiceUnavailable, $"Unable to {method} item");
  }
  private static System.Exception notFoundError()
  {
    throw new AppError(HttpStatusCode.NotFound, "This item not exists");
  }

  public Todo GetById(int id)
  {
    Todo? targetTodo = null;

    try
    {
      var todo = _context
        .Todos
        .FirstOrDefault(todo => todo.Id == id);
      targetTodo = todo;
    }
    catch
    {
      serviceUnavailable("getById");
    }
    
    if (targetTodo == null)
    {
      throw notFoundError();
    }

    return targetTodo;
  }
  public List<Todo> List()
  {
    List<Todo>? todos = new();

    try
    {
      todos = _context
        .Todos
        .ToList();
    }
    catch
    {
      serviceUnavailable("list");
    }

    if (todos is null)
    {
      throw notFoundError();
    }
    return todos.ToList();
  }
  public Todo create(CreateTodoViewModel todo)
  {
    var todoInstance = new Todo { Title = todo.Title, Done = false };
    try
    {
      _context.Todos.Add(todoInstance);
      _context.SaveChanges();
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
      throw new AppError(HttpStatusCode.InternalServerError, "Is not possible create item");
    }
    return todoInstance;
  }
  public Todo EditTodo(int id, CreateTodoViewModel todo)
  {
    var targetTodo = _context
      .Todos
      .FirstOrDefault(todo => todo.Id == id);

    if (targetTodo is null)
    {
      throw notFoundError();
    }
    targetTodo.Title = todo.Title;

    try
    {
      _context.Todos.Update(targetTodo);
      _context.SaveChanges();
    }
    catch
    {
      serviceUnavailable("edit");
    }
    
    return targetTodo;
  }
  public Todo DeleteTodo(int id)
  {
    var todo = _context
      .Todos
      .FirstOrDefault(currentTodo => currentTodo.Id == id);
    
    if(todo == null) throw notFoundError();

    try
    {
      _context.Remove(todo);
      _context.SaveChanges();
    }
    catch
    {
      serviceUnavailable("delete");
    }
    
    return todo;
  }
}