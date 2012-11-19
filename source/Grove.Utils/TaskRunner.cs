namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class TaskRunner
  {    
    private readonly Dictionary<string, Action<Arguments>> _registration = new Dictionary<string, Action<Arguments>>();  
    private readonly Tasks _tasks = new Tasks();

    protected static readonly IoC Container = CreateContainer();

    private static IoC CreateContainer()
    {
      var container = IoC.Test();
      container.Register(typeof(Tasks));
      return container;
    }

    public TaskRunner()
    {
      _tasks = Container.Resolve<Tasks>();
      _registration = Define();
    }

    private TaskRegistration Task(string keyword, Action<Arguments> task)
    {
      return new TaskRegistration(keyword, task);
    }

    private Dictionary<string, Action<Arguments>> Define()
    {
                  
      var entries = new[]
        {
          Task("write-card-list", arguments => _tasks.WriteCardList(arguments["filename"]))
        };

      return entries.ToDictionary(x => x.Keyword, x => x.Task);
    }

    public void Run(string name, string[] args)
    {
      if (_registration.ContainsKey(name))
      {

        try
        {
          _registration[name](new Arguments(args));
          Console.WriteLine("Task completed successfuly.");
        }
        catch(Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

        return;
      }

      Console.WriteLine(String.Format("Invalid task name: '{0}'.", name));
    }   
  }
}