#pragma warning disable 1591
namespace MyTodo.Models {
  public class Todo {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
    public DateTime DateTIme { get; set; } = DateTime.UtcNow;
  }
}