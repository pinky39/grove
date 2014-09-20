namespace Grove
{
  using Infrastructure;

  [Copyable]
  public class ManaUnit
  {
    public readonly ManaColor Color;
    public readonly int Rank;
    public readonly IManaSource Source;
    private readonly ManaUsage _usageRestriction;

    private ManaUnit() {}

    public ManaUnit(ManaColor color, int rank, IManaSource source = null, ManaUsage usageRestriction = ManaUsage.Any)
    {
      Color = color;
      Rank = rank;
      Source = source;
      _usageRestriction = usageRestriction;
    }

    public bool HasSource { get { return Source != null; } }

    public bool CanActivateSource()
    {
      return Source != null && Source.CanActivate();
    }

    public bool CanBeUsed(ManaUsage usage)
    {
      return _usageRestriction == ManaUsage.Any ||
        _usageRestriction == usage;
    }
  }
}