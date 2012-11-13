namespace Grove.Core.Mana
{
  using System;
  using System.Linq;
  using Grove.Infrastructure;

  [Copyable]
  public class ManaPool : IManaSource
  {
    private readonly ManaBag _restrictedAbilities;
    private readonly ManaBag _unrestricted;

    private ManaPool() {}

    public ManaPool(ChangeTracker changeTracker)
    {
      _unrestricted = new ManaBag(changeTracker);
      _restrictedAbilities = new ManaBag(changeTracker);
    }

    public int WhiteCount { get { return CountTotal(ManaColors.White); } }

    public int BlueCount { get { return CountTotal(ManaColors.Blue); } }
    public int BlackCount { get { return CountTotal(ManaColors.Black); } }
    public int RedCount { get { return CountTotal(ManaColors.Red); } }
    public int GreenCount { get { return CountTotal(ManaColors.Green); } }
    public int MultiCount { get { return CountTotal(x => x.IsMultiColor); } }
    public int ColorlessCount { get { return CountTotal(x => x.IsColorless); } }    

    public int Priority { get { return 0; } }
    object IManaSource.Resource { get { return this; } }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount", "ColorlessCount")]
    public virtual void Consume(IManaAmount amount, ManaUsage usage)
    {
      // since the check will be made if pool contains
      // appropriate mana, we can speed things up a
      // bit if the whole pool should be consumed
      if (amount.Converted == _unrestricted.Count + _restrictedAbilities.Count)
      {
        _unrestricted.Clear();
        _restrictedAbilities.Clear();
        return;
      }

      if ((usage & ManaUsage.Abilities) == ManaUsage.Abilities)
      {
        _restrictedAbilities.Consume(amount);
      }

      _unrestricted.Consume(amount);
    }

    public IManaAmount GetAvailableMana(ManaUsage usage)
    {
      if ((usage & ManaUsage.Abilities) != ManaUsage.Abilities)
      {
        return _unrestricted.GetAmount();
      }

      return new AggregateManaAmount(_unrestricted.GetAmount(), _restrictedAbilities.GetAmount());
    }

    private int CountTotal(ManaColors color)
    {
      return _unrestricted.Count(x => x.IsSingleColor(color)) +
        _restrictedAbilities.Count(x => x.IsSingleColor(color));
    }

    private int CountTotal(Func<ManaUnit, bool> selector)
    {
      return _unrestricted.Count(selector) + _restrictedAbilities.Count(selector);
    }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount", "ColorlessCount")]
    public virtual void Add(IManaAmount amount)
    {
      _unrestricted.Add(amount);
    }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount", "ColorlessCount")]
    public virtual void AddAbilities(IManaAmount amount)
    {
      _restrictedAbilities.Add(amount);
    }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount", "ColorlessCount")]
    public virtual void Empty()
    {
      _unrestricted.Clear();
      _restrictedAbilities.Clear();
    }

    public override string ToString()
    {
      return _unrestricted.ToString();
    }
  }
}