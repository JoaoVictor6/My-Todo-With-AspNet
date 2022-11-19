using System.Net;
using MyTodo.Utils;
using Newtonsoft.Json;
# pragma warning disable CS1591

namespace MyTodo.Middlewares;

[Serializable] 
public class ErrorHandling: Exception
{
  private readonly RequestDelegate _next;

  public ErrorHandling(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
      await HandleExceptionAsync(context, err);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception err)
  {
    if (err is AppError)
    {
      var error = (AppError)err;
      context.Response.StatusCode = (int)error.Code;
      var appErrorBody = JsonConvert.SerializeObject(new
      {
        code = error.Code,
        messsage = error.Message
      });
      context.Response.ContentType = "application/json";
      Console.WriteLine($"Error > {error.Timestamp} | {error.Message} | {(int)error.Code} ");
      return context.Response.WriteAsync(appErrorBody);
    }
    
    Console.WriteLine(err.Message);
    var defaultErrorJson = JsonConvert.SerializeObject(new { error = err.Message });
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    return context.Response.WriteAsync(defaultErrorJson);
  }
}