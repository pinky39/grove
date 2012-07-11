namespace Grove.Core.Details.Mana
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaBag : IEnumerable<ManaUnit>
  {
    private readonly IList<ManaUnit> _bag;

    public ManaBag(IEnumerable<ManaUnit> amount, ChangeTracker changeTracker)
    {
      _bag = new TrackableList<ManaUnit>(amount, changeTracker);
    }

    public ManaBag(ChangeTracker changeTracker)
    {
      _bag = new TrackableList<ManaUnit>(changeTracker);
    }

    public ManaBag(IEnumerable<ManaUnit> amount)
    {
      _bag = new List<ManaUnit>(amount);
    }

    public ManaBag()
    {
      _bag = new List<ManaUnit>();
    }

    public IManaAmount Amount { get { return new PrimitiveManaAmount(_bag); } }
    public int Count { get { return _bag.Count; } }
    public bool IsEmpty { get { return _bag.Count == 0; } }

    public IEnumerator<ManaUnit> GetEnumerator()
    {
      return _bag.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(IManaAmount manaAmount)
    {
      foreach (var mana in manaAmount)
      {
        _bag.Add(mana);
      }
    }

    public void Clear()
    {
      _bag.Clear();
    }

    public void Consume(IManaAmount amount)
    {
      foreach (var mana in amount)
      {
        var removed = _bag.Remove(mana);

        if (!removed)
        {
          throw new InvalidOperationException(
            String.Format("Mana {0} not found.", mana));
        }
      }
    }

    public override string ToString()
    {
      if (_bag.Count == 0)
        return "No mana";

      return string.Join(",", _bag.Select(x => x.ToString()));
    }
  }
}