# pragma warning disable CS1591
namespace MyTodo.DotEnv {
  public static class DotEnv {
    public static void LoadEnvs(string filePath) {
      if(!File.Exists(filePath))
        return;
      
      foreach (var line in File.ReadAllLines(filePath)) 
      {
        var parts = line.Split(
          '=', 
          StringSplitOptions.RemoveEmptyEntries
        );

        if(parts.Length != 2)
          continue;
        Environment.SetEnvironmentVariable(parts[0], parts[1]);
      }
    }

    public static void Load() {
      var appRoot = Directory.GetCurrentDirectory();
      var dotEnv = Path.Combine(appRoot, ".env");

      LoadEnvs(dotEnv);
    }
  }
}