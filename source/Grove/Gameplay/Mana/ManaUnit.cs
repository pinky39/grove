namespace Grove.Gameplay.Mana
{
  using Grove.Infrastructure;

  [Copyable]
  public class ManaUnit
  {
    private readonly ManaUsage _usageRestriction;

    private ManaUnit() {}

    public ManaUnit(ManaColor color, int rank, IManaSource source = null,
      object tapRestriction = null, int costRestriction = 0, ManaUsage usageRestriction = ManaUsage.Any)
    {
      Color = color;
      Rank = rank;
      Source = source;
      TapRestriction = tapRestriction;
      CostRestriction = costRestriction;
      _usageRestriction = usageRestriction;
    }

    public int CostRestriction { get; private set; }
    public IManaSource Source { get; private set; }
    public bool HasSource { get { return Source != null; } }
    public ManaColor Color { get; private set; }
    public int Rank { get; private set; }
    public object TapRestriction { get; private set; }

    public bool CanActivateSource()
    {
      if (Source == null)
        return false;

      return Source.CanActivate();
    }

    public bool CanBeUsed(ManaUsage usage)
    {
      if (_usageRestriction != ManaUsage.Any && _usageRestriction != usage)
        return false;

      return true;
    }
  }
}