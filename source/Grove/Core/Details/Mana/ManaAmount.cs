namespace Grove.Core.Details.Mana
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

    public static IManaAmount Any { get { return ManaUnit.Any.ToAmount(); } }
    public static IManaAmount White { get { return ManaUnit.White.ToAmount(); } }
    public static IManaAmount Black { get { return ManaUnit.Black.ToAmount(); } }
    public static IManaAmount Blue { get { return ManaUnit.Blue.ToAmount(); } }
    public static IManaAmount Red { get { return ManaUnit.Red.ToAmount(); } }
    public static IManaAmount Green { get { return ManaUnit.Green.ToAmount(); } }

    public static IEnumerable<ManaUnit> Colored(this IManaAmount amount)
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

    public static string ToString(this IManaAmount amount)
    {
      var sb = new StringBuilder();

      
      foreach (var symbolName in amount.GetSymbolNames())
      {
        sb.Append("{");
        sb.Append(symbolName);
        sb.Append("}");
      }

      return sb.ToString();
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
        Enumerable.Repeat(new ManaUnit(), amount));
    }

    private static int WubrgOrdering(ManaUnit mana)
    {
      return mana.EnumerateColors().Aggregate(1, (sum, color) => sum + ((int) color*10));
    }


    public static IManaAmount ParseManaAmount(this string str)
    {
      if (str == null)
        return null;

      str = str.ToLowerInvariant();
      var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);
      var parsed = new List<ManaUnit>();

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
        
    public static List<string> GetSymbolsFromColor(ManaColors color)
    {
      var list = new List<string>();

      foreach (var colorChar in Symbols)
      {
        if ((colorChar.Color & color) == colorChar.Color)
          list.Add(colorChar.Char.ToString());
      }

      return list;
    }

    private static ManaUnit ParseColored(string token)
    {
      var color = ManaColors.None;

      foreach (var ch in token)
      {
        color = color | GetColorFromSymbol(ch);
      }

      if (color == ManaColors.Colorless)
        throw new InvalidOperationException("Invalid color token: " + token);

      return new ManaUnit(color);
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