namespace Grove.Core
{
  using Infrastructure;

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
    
    public ManaAmount Amount { get { return _manaBag.Amount; } }

    public bool IsEmpty { get { return _manaBag.IsEmpty; } }

    public int Priority { get { return 0; } }
    object IManaSource.Resource { get { return this; } }

    [Updates("Amount")]
    public virtual void Consume(ManaAmount amount)
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

    public ManaAmount GetAvailableMana()
    {
      return _manaBag.Amount;
    }

    [Updates("Amount")]
    public virtual void Add(ManaAmount amount)
    {
      _manaBag.Add(amount);
    }

    [Updates("Amount")]
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