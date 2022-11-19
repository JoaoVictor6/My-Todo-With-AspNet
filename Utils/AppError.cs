#pragma warning disable 1591
using System.Net;
namespace MyTodo.Utils;

public interface IAppError{
  public DateTime Timestamp { get; protected set; }
  public HttpStatusCode Code { get; protected set; }
  public string Message { get; }
}

public class AppError : Exception, IAppError
{
  public AppError(HttpStatusCode status, string message)
  {
    Code = status;
    Message = message;
  }

  public override string Message { get; }
  public DateTime Timestamp { get; set; }
  public HttpStatusCode Code { get; set; }
}