namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using Infrastructure;

  public class Mana : IEquatable<Mana>
  {
    private static readonly ColorChar[] ColorChars = new[]{
      new ColorChar{Symbol = 'w', Color = ManaColors.White},
      new ColorChar{Symbol = 'u', Color = ManaColors.Blue},
      new ColorChar{Symbol = 'b', Color = ManaColors.Black},
      new ColorChar{Symbol = 'r', Color = ManaColors.Red},
      new ColorChar{Symbol = 'g', Color = ManaColors.Green},
    };

    private readonly ManaColors _colors;

    public Mana(ManaColors colors = ManaColors.Colorless)
    {
      _colors = colors;
    }

    public static ManaAmount Any { get { return new Mana(ManaColors.White | ManaColors.Blue | ManaColors.Black | ManaColors.Red | ManaColors.Green); } }

    public static Mana Black { get { return new Mana(ManaColors.Black); } }

    public static Mana Blue { get { return new Mana(ManaColors.Blue); } }
    public ManaColors Colors { get { return _colors; } }

    public static Mana Green { get { return new Mana(ManaColors.Green); } }

    public bool IsColored { get { return !IsColorless; } }

    public bool IsColorless { get { return _colors == ManaColors.Colorless; } }
    public bool IsMultiColor { get { return Rank > 1; } }

    public int Order { get { return (int) _colors; } }
    public int Rank { get { return EnumEx.GetSetBitCount((long) _colors); } }

    public static Mana Red { get { return new Mana(ManaColors.Red); } }
    public string Symbol { get { return GetSymbolFromColor(_colors); } }

    public static Mana White { get { return new Mana(ManaColors.White); } }

    public bool Equals(Mana other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other._colors, _colors);
    }

    public static ManaAmount Parse(string str)
    {
      str = str.ToLowerInvariant();
      var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);
      var manaBag = new ManaBag();

      foreach (var token in tokens)
      {
        var parsed =
          ParseColorless(token) ??
            ParseColored(token);

        manaBag.Add(parsed);
      }

      return manaBag.Amount;
    }

    public IEnumerable<ManaColors> EnumerateColors()
    {
      if (HasColor(ManaColors.White))
        yield return ManaColors.White;

      if (HasColor(ManaColors.Blue))
        yield return ManaColors.Blue;

      if (HasColor(ManaColors.Black))
        yield return ManaColors.Black;

      if (HasColor(ManaColors.Red))
        yield return ManaColors.Red;

      if (HasColor(ManaColors.Green))
        yield return ManaColors.Green;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (Mana)) return false;
      return Equals((Mana) obj);
    }

    public override int GetHashCode()
    {
      return _colors.GetHashCode();
    }

    public bool HasColor(ManaColors color)
    {
      return (_colors & color) == color;
    }

    public bool IsSingleColor(ManaColors color)
    {
      return HasColor(color) && Rank == 1;
    }

    public override string ToString()
    {
      return String.Format("{{{0}}}", _colors);
    }

    private static ManaColors GetColorFromSymbol(char code)
    {
      foreach (var colorChar in ColorChars)
      {
        if (code == colorChar.Symbol)
          return colorChar.Color;
      }

      return ManaColors.Colorless;
    }

    private static string GetSymbolFromColor(ManaColors color)
    {
      var sb = new StringBuilder();

      foreach (var colorChar in ColorChars)
      {
        if (color == colorChar.Color)
          sb.Append(colorChar.Symbol);
      }

      return sb.ToString();
    }

    private static ManaAmount ParseColored(string token)
    {
      var color = ManaColors.None;

      foreach (var ch in token)
      {
        color = color | GetColorFromSymbol(ch);
      }

      if (color == ManaColors.Colorless)
        throw new InvalidOperationException("Invalid color token: " + token);

      return new ManaAmount(new Mana(color));
    }

    private static ManaAmount ParseColorless(string token)
    {
      int count;
      if (Int32.TryParse(token, out count))
        return ManaAmount.Colorless(count);

      return null;
    }

    public static bool operator ==(Mana left, Mana right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Mana left, Mana right)
    {
      return !Equals(left, right);
    }

    private class ColorChar
    {
      public ManaColors Color { get; set; }
      public char Symbol { get; set; }
    }
  }
}