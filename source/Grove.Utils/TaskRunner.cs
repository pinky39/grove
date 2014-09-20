namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;

  public class TaskRunner
  {
    protected static readonly IoC Container = CreateContainer();
    private readonly Dictionary<string, Func<Task>> commands = new Dictionary<string, Func<Task>>();

    public TaskRunner()
    {
      commands = new Dictionary<string, Func<Task>>
        {
          {"list", () => Container.Resolve<WriteCardList>()},
          {"rate", () => Container.Resolve<WriteCardList>()},
          {"gen", () => Container.Resolve<GenerateDecks>()},
          {"debug", () => Container.Resolve<ReproduceError>()},
        };
    }

    private static IoC CreateContainer()
    {
      var container = new IoC();
      return container;
    }

    public bool Run(string[] args)
    {
      if (args.Length < 1)
        return false;

      if (args[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
      {
        return ShowCommandUsage(args);
      }

      var command = args[0];

      if (commands.ContainsKey(command))
      {
        return commands[command]().Execute(new Arguments(args));        
      }

      WriteInvalidCommand(command);
      return false;
    }

    private bool ShowCommandUsage(string[] args)
    {
      if (args.Length < 2)
        return false;

      var command = args[1].ToLowerInvariant();

      if (!commands.ContainsKey(command))
      {
        WriteInvalidCommand(command);
        return false;
      }

      commands[command]().Usage();
      return true;
    }

    private void WriteInvalidCommand(string command)
    {
      Console.WriteLine(String.Format("Invalid command name: '{0}'.", command));
    }
  }
}