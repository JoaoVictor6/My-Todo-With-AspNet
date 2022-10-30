using System.ComponentModel.DataAnnotations;

namespace MyTodo.ViewModels {
  public class CreateTodoViewModel {
    [Required]
    public string Title {get; set;}
  }
}