namespace Grove.Core.Mana
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaBag : IEnumerable<ManaUnit>
  {
    private IList<ManaUnit> _bag;

    public ManaBag(IEnumerable<ManaUnit> amount)
    {
      _bag = new List<ManaUnit>(amount);
    }

    public ManaBag()
    {
      _bag = new List<ManaUnit>();
    }

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

    public IManaAmount GetAmount()
    {
      return new PrimitiveManaAmount(_bag);
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      var nonTrackable = _bag;
      _bag = new List<ManaUnit>(nonTrackable);
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
        _bag.Remove(mana);
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