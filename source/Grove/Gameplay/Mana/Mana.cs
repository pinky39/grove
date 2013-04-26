namespace Grove.Gameplay.Mana
{
  public static class Mana
  {
    public static readonly IManaAmount Zero = new ZeroManaAmount();
    public static readonly IManaAmount Any = new SingleColorManaAmount(ManaColor.Any, 1);
    public static readonly IManaAmount White = new SingleColorManaAmount(ManaColor.White, 1);
    public static readonly IManaAmount Blue = new SingleColorManaAmount(ManaColor.Blue, 1);
    public static readonly IManaAmount Black = new SingleColorManaAmount(ManaColor.Black, 1);
    public static readonly IManaAmount Red = new SingleColorManaAmount(ManaColor.Red, 1);
    public static readonly IManaAmount Green = new SingleColorManaAmount(ManaColor.Green, 1);

    public static IManaAmount Colored(bool isWhite = false, bool isBlue = false, bool isBlack = false,
      bool isRed = false, bool isGreen = false, int count = 1)
    {
      return new SingleColorManaAmount(new ManaColor(isWhite, isBlue, isBlack, isRed, isGreen), count);
    }

    public static IManaAmount Colored(ManaColor color, int count)
    {
      if (count == 0)
        return Zero;

      return new SingleColorManaAmount(color, count);
    }

    public static IManaAmount Parse(this string str)
    {
      return ManaParser.ParseMana(str);
    }

    public static IManaAmount Colorless(this int value)
    {
      return new SingleColorManaAmount(ManaColor.Colorless, value);
    }

    public static IManaAmount Repeat(this IManaAmount amount, int count)
    {
      var result = amount;

      for (var i = 1; i < count; i++)
      {
        result = result.Add(amount);
      }

      return result;
    }

    public static IManaAmount GetBasicLandMana(string name)
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