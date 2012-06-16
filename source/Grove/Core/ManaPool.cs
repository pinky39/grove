namespace Grove.Core
{
  using Infrastructure;
  using System.Linq;

  [Copyable]
  public class ManaPool : IManaSource
  {
    private readonly ManaBag _manaBag;

    private ManaPool()
    {      
    }
    
    public ManaPool(ChangeTracker changeTracker)
    {
      _manaBag = new ManaBag(changeTracker); 
    }
    
    public int WhiteCount
    {
      get { return _manaBag.Count(x => x.IsSingleColor(ManaColors.White)); }
    }

    public int BlueCount
    {
      get { return _manaBag.Count(x => x.IsSingleColor(ManaColors.Blue)); }
    }

    public int BlackCount
    {
      get { return _manaBag.Count(x => x.IsSingleColor(ManaColors.Black)); }
    }

    public int RedCount
    {
      get { return _manaBag.Count(x => x.IsSingleColor(ManaColors.Red)); }
    }

    public int GreenCount
    {
      get { return _manaBag.Count(x => x.IsSingleColor(ManaColors.Green)); }
    }

    public int MultiCount
    {
      get { return _manaBag.Count(x => x.IsMultiColor); }
    }

    public int ColorlessCount
    {
      get { return _manaBag.Count(x => x.IsColorless); }
    }

    public bool IsEmpty { get { return _manaBag.IsEmpty; } }

    public int Priority { get { return 0; } }
    object IManaSource.Resource { get { return this; } }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount")]
    public virtual void Consume(IManaAmount amount)
    {
      // since the check will be made if pool contains
      // appropriate mana, we can speed things up a
      // bit if the whole pool should be consumed
      if (amount.Converted == _manaBag.Count)
      {
        _manaBag.Clear();
        return;
      }

      _manaBag.Consume(amount);
    }

    public IManaAmount GetAvailableMana()
    {
      return _manaBag.Amount;
    }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount")]
    public virtual void Add(IManaAmount amount)
    {
      _manaBag.Add(amount);
    }

    [Updates("WhiteCount", "BlueCount", "BlackCount", "RedCount", "GreenCount", "MultiCount")]
    public virtual void Empty()
    {
      _manaBag.Clear();
    }

    public override string ToString()
    {
      return _manaBag.ToString();
    }
  }
}