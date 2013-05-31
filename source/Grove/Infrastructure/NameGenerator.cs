namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class NameGenerator
  {    
    private List<string> _unitNames = new List<string>();

    public NameGenerator(string filename)
    {
      LoadFromFile(filename);
    }

    private void LoadFromFile(string filename)
    {
      var rows = File.ReadAllLines(filename);

      foreach (var row in rows)
      {
        var trimmed = row.Trim();

        if (trimmed.StartsWith("#"))
          continue;

        if (String.IsNullOrEmpty(trimmed))
          continue;

        _unitNames.Add(trimmed);
      }
    }

    public string[] GenerateNames(int count)
    {
      var generated = new HashSet<string>();

      for (var i = 0; i < count; i++)
      {
        while (true)
        {
          var name = GenerateName();

          if (generated.Contains(name) == false)
          {
            generated.Add(name);
            break;
          }
        }
      }

      return generated.ToArray();
    }

    public string GenerateName()
    {
      var first = _unitNames[RandomEx.Next(_unitNames.Count)];
      var second = _unitNames[RandomEx.Next(_unitNames.Count)];

      return first + " " + second;
    }
  }
}