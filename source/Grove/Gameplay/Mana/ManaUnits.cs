namespace Grove.Gameplay.Mana
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaUnits
  {
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();

    public List<ManaUnit> GetIf(Func<ManaUnit, bool> filter, Func<ManaUnit, int> order = null)
    {
      var unrestricted = _units
        .Where(filter);

      if (order != null)
      {
        unrestricted = unrestricted.OrderBy(order);
      }

      var restrictions = new Dictionary<object, IManaSource>();
      var restricted = new List<ManaUnit>();

      foreach (var unit in unrestricted)
      {
        if (unit.TapRestriction == null)
        {
          restricted.Add(unit);
          continue;
        }

        IManaSource source;
        if (restrictions.TryGetValue(unit.TapRestriction, out source))
        {
          if (source != unit.Source)
            continue;

          restricted.Add(unit);
          continue;
        }

        restrictions.Add(unit.TapRestriction, unit.Source);
        restricted.Add(unit);
      }

      return restricted;
    }

    public IEnumerable<ManaUnit> GetIf(int minCount, Func<ManaUnit, bool> filter, Func<ManaUnit, int> order)
    {
      var unlimited = GetIf(filter, order);

      return unlimited.Count >= minCount
        ? unlimited.Take(minCount)
        : null;
    }

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      _units.Initialize(changeTracker);
    }

    public void Add(ManaUnit unit)
    {
      _units.Add(unit);
    }

    public void Remove(ManaUnit unit)
    {
      _units.Remove(unit);
    }
  }
}