namespace Grove
{
  public static class Mana
  {    
    public static readonly ManaAmount Zero = new ZeroManaAmount();
    public static readonly ManaAmount Any = new SingleColorManaAmount(ManaColor.Any, 1);
    public static readonly ManaAmount White = new SingleColorManaAmount(ManaColor.White, 1);
    public static readonly ManaAmount Blue = new SingleColorManaAmount(ManaColor.Blue, 1);
    public static readonly ManaAmount Black = new SingleColorManaAmount(ManaColor.Black, 1);
    public static readonly ManaAmount Red = new SingleColorManaAmount(ManaColor.Red, 1);
    public static readonly ManaAmount Green = new SingleColorManaAmount(ManaColor.Green, 1);

    public static ManaAmount Colored(bool isWhite = false, bool isBlue = false, bool isBlack = false,
      bool isRed = false, bool isGreen = false, int count = 1)
    {
      return new SingleColorManaAmount(new ManaColor(isWhite, isBlue, isBlack, isRed, isGreen), count);
    }

    public static ManaAmount Colored(ManaColor color, int count)
    {
      if (count == 0)
        return Zero;

      return new SingleColorManaAmount(color, count);
    }

    public static ManaAmount Parse(this string str)
    {
      return ManaParser.ParseMana(str);
    }

    public static ManaAmount Colorless(this int value)
    {
      return new SingleColorManaAmount(ManaColor.Colorless, value);
    }

    public static ManaAmount Repeat(this ManaAmount amount, int count)
    {
      var result = amount;

      for (var i = 1; i < count; i++)
      {
        result = result.Add(amount);
      }

      return result;
    }

    public static ManaAmount GetBasicLandMana(string name)
    {
      switch (name.ToLowerInvariant())
      {
        case ("plains"):
          return White;
        case ("island"):
          return Blue;
        case ("swamp"):
          return Black;
        case ("mountain"):
          return Red;
        case ("forest"):
          return Green;
      }

      return Colorless(1);
    }
  }
}