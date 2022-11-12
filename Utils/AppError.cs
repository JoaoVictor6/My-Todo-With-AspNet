using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Openapi.Weibo.Model;

namespace MyTodo.Utils;

public class AppError : SystemException
{
  public DateTime Timestamp { get; set; } = DateTime.Now;
  public HttpStatusCode Code { get; set; }
  public string Message { get; set; }

  public AppError(HttpStatusCode status, string message)
  {
    Code = status;
    Message = message;
  }
}