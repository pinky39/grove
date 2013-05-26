namespace Grove.Gameplay.ManaHandling
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

      var tapRestrictions = new Dictionary<object, IManaSource>();
      var sacRestrictions = new Dictionary<object, IManaSource>();
      var restricted = new List<ManaUnit>();

      foreach (var unit in unrestricted)
      {
        if (unit.TapRestriction != null)
        {
          IManaSource source;
          if (tapRestrictions.TryGetValue(unit.TapRestriction, out source))
          {
            if (source != unit.Source)
              continue;
          }
          else
          {
            tapRestrictions.Add(unit.TapRestriction, unit.Source);
          }
        }
        else if (unit.SacRestriction != null)
        {
          IManaSource source;
          if (sacRestrictions.TryGetValue(unit.SacRestriction, out source))
          {
            if (source != unit.Source)
              continue;
          }
          else
          {
            sacRestrictions.Add(unit.SacRestriction, unit.Source);
          }
        }

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