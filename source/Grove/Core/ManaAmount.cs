namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;

  public static class ManaAmount
  {
    private static readonly Symbol[] Symbols = new[]
      {
        new Symbol {Char = 'W', Color = ManaColors.White},
        new Symbol {Char = 'U', Color = ManaColors.Blue},
        new Symbol {Char = 'B', Color = ManaColors.Black},
        new Symbol {Char = 'R', Color = ManaColors.Red},
        new Symbol {Char = 'G', Color = ManaColors.Green},
      };

    public static readonly ZeroManaAmount Zero = new ZeroManaAmount();

    public static IManaAmount Any
    {
      get { return Mana.Any.ToAmount(); }
    }

    public static IManaAmount White
    {
      get { return Mana.White.ToAmount(); }
    }

    public static IManaAmount Black
    {
      get { return Mana.Black.ToAmount(); }
    }

    public static IManaAmount Blue
    {
      get { return Mana.Blue.ToAmount(); }
    }

    public static IManaAmount Red
    {
      get { return Mana.Red.ToAmount(); }
    }

    public static IManaAmount Green
    {
      get { return Mana.Green.ToAmount(); }
    }

    public static IEnumerable<Mana> Colored(this IManaAmount amount)
    {
      return amount.Where(mana => mana.IsColored);
    }

    public static bool IsZero(this IManaAmount amount)
    {
      return amount.Converted == 0;
    }

    public static int ColorlessCount(this IManaAmount amount)
    {
      return amount.Count(mana => mana.IsColorless);
    }

    public static int MaxRank(this IManaAmount amount)
    {
      return amount.IsZero() ? 0 : amount.Max(x => x.Rank);
    }    
    
    public static string[] GetSymbolNames(this IManaAmount amount)
    {
      var colorlessCount = amount.Count(mana => mana.IsColorless);

      var names = new List<string>();

      if (colorlessCount > 0)
      {
        names.Add(colorlessCount.ToString());
      }

      names.AddRange(amount.Colored().OrderBy(WubrgOrdering).Select(mana => mana.Symbol));
      return names.ToArray();
    }

    public static bool HasColor(this IManaAmount manaAmount, ManaColors color)
    {
      return manaAmount.Any(x => x.HasColor(color));
    }

    public static IManaAmount Add(this IManaAmount amount1, int amount2)
    {
      return new AggregateManaAmount(amount1, amount2.AsColorlessMana());
    }

    public static IManaAmount Add(this IManaAmount amount1, IManaAmount amount2)
    {
      return new AggregateManaAmount(amount1, amount2);
    }

    public static IManaAmount AsColorlessMana(this int amount)
    {
      return Colorless(amount);
    }

    public static IManaAmount Colorless(int amount)
    {
      return new PrimitiveManaAmount(
        Enumerable.Repeat(new Mana(), amount));
    }

    private static int WubrgOrdering(Mana mana)
    {
      return mana.EnumerateColors().Aggregate(1, (sum, color) => sum + ((int) color*10));
    }


    public static IManaAmount ParseManaAmount(this string str)
    {
      if (str == null)
        return null;

      str = str.ToLowerInvariant();
      var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);
      var parsed = new List<Mana>();

      foreach (var token in tokens)
      {
        var colorless = ParseColorless(token);

        if (colorless.HasValue)
        {
          parsed.AddRange(
            Colorless(colorless.Value));

          continue;
        }

        var mana = ParseColored(token);

        parsed.Add(mana);
      }

      return new PrimitiveManaAmount(parsed);
    }

    public static string GetSymbolsFromColor(ManaColors color)
    {
      var sb = new StringBuilder();

      foreach (var colorChar in Symbols)
      {
        if ((colorChar.Color & color) == colorChar.Color)
          sb.Append(colorChar.Char);
      }

      return sb.ToString();
    }

    private static Mana ParseColored(string token)
    {
      var color = ManaColors.None;

      foreach (var ch in token)
      {
        color = color | GetColorFromSymbol(ch);
      }

      if (color == ManaColors.Colorless)
        throw new InvalidOperationException("Invalid color token: " + token);

      return new Mana(color);
    }

    public static ManaColors GetColorFromSymbol(char code)
    {
      foreach (var colorChar in Symbols)
      {
        if (Char.ToUpper(code) == colorChar.Char)
          return colorChar.Color;
      }

      return ManaColors.Colorless;
    }

    private static int? ParseColorless(string token)
    {
      int count;
      if (Int32.TryParse(token, out count))
        return count;

      return null;
    }

    private class Symbol
    {
      public ManaColors Color { get; set; }
      public char Char { get; set; }
    }
  }
}