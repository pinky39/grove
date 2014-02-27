namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  public class ManaParser
  {
    public static IManaAmount ParseMana(string str)
    {
      str = str.ToLowerInvariant();
      var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);

      var parsed = new Dictionary<ManaColor, int>();

      foreach (var token in tokens)
      {
        var colorless = ParseColorless(token);

        if (colorless.HasValue)
        {
          parsed[ManaColor.Colorless] = colorless.Value;
          continue;
        }

        var color = ParseColored(token);

        if (parsed.ContainsKey(color))
        {
          parsed[color]++;
        }
        else
        {
          parsed[color] = 1;
        }
      }

      if (parsed.Count == 1)
        return new SingleColorManaAmount(parsed.Keys.First(), parsed.Values.First());

      return new MultiColorManaAmount(parsed);
    }

    private static ManaColor ParseColored(string token)
    {
      bool isWhite = false, isBlue = false, isBlack = false, isRed = false, isGreen = false;

      foreach (var ch in token)
      {
        switch (Char.ToUpper(ch))
        {
          case ('W'):
            isWhite = true;
            break;
          case ('U'):
            isBlue = true;
            break;
          case ('B'):
            isBlack = true;
            break;
          case ('R'):
            isRed = true;
            break;
          case ('G'):
            isGreen = true;
            break;
        }
      }

      return new ManaColor(isWhite, isBlue, isBlack, isRed, isGreen);
    }

    private static int? ParseColorless(string token)
    {
      int count;
      if (Int32.TryParse(token, out count))
        return count;

      return null;
    }
  }
}