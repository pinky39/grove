namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  public static class NameGenerator
  {       
    public static string[] GenerateRandomNames(IList<string> unitNames, int count)
    {
      var generated = new HashSet<string>();

      for (var i = 0; i < count; i++)
      {
        while (true)
        {
          var name = GenerateRandomName(unitNames);

          if (generated.Contains(name) == false)
          {
            generated.Add(name);
            break;
          }
        }
      }

      return generated.ToArray();
    }

    public static string GenerateRandomName(IList<string> unitNames)
    {
      var first = unitNames[RandomEx.Next(unitNames.Count)];
      var second = unitNames[RandomEx.Next(unitNames.Count)];

      return first + " " + second;
    }
  }
}