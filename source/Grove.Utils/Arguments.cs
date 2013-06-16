namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;

  public class Arguments
  {
    private static readonly Regex NamedArgument = new Regex(@"(.+)=(.+)", RegexOptions.Compiled);
    private Dictionary<string, string> _arguments = new Dictionary<string, string>();

    public Arguments(string[] args)
    {
      foreach (var arg in args)
      {
        var match = NamedArgument.Match(arg);

        if (match.Success)
        {
          _arguments.Add(
            match.Groups[1].Value.Trim(),
            match.Groups[2].Value.Trim());
        }
      }
    }

    public string this[string name]
    {
      get
      {
        if (_arguments.ContainsKey(name))
          return _arguments[name];

        throw new InvalidOperationException(
          String.Format("Required argument '{0}' not specified.", name));
      }
    }

    public string TryGet(string name)
    {
      if (_arguments.ContainsKey(name))
        return _arguments[name];

      return null;
    }
  }
}