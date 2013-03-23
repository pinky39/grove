namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;

  public class TaskRunner
  {
    protected static readonly IoC Container = CreateContainer();
    private readonly Dictionary<string, Func<Task>> _registrations = new Dictionary<string, Func<Task>>();

    public TaskRunner()
    {
      _registrations = new Dictionary<string, Func<Task>>()
        {
          {"write-card-list", () => Container.Resolve<WriteCardList>()},
          {"write-card-ratings", () => Container.Resolve<WriteCardList>()},
          {"generate-deck", () => Container.Resolve<GenerateDeck>()}
        };
    }

    private static IoC CreateContainer()
    {
      var container = new IoC();
      return container;
    }

    public void Run(string name, string[] args)
    {
      if (_registrations.ContainsKey(name))
      {
        try
        {
          _registrations[name]().Execute(new Arguments(args));
          Console.WriteLine("Task completed successfuly.");
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

        return;
      }

      Console.WriteLine(String.Format("Invalid task name: '{0}'.", name));
    }
  }
}