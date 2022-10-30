namespace MyTodo.Models {
  public class Todo {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Done { get; set; }
    public DateTime DateTIme { get; set; } = DateTime.Now;
  }
}