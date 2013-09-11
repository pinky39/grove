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

    public int Count { get { return _units.Count; } }
    
    public bool TryToAllocate(Func<ManaUnit, bool> filter, out List<ManaUnit> allocated, int? count = null, Func<ManaUnit, int> order = null)
    {
      allocated = null;
                 
      var tapRestrictions = new Dictionary<object, IManaSource>();
      var sacRestrictions = new Dictionary<object, IManaSource>();
      var restricted = new List<ManaUnit>();

      foreach (var unit in _units)
      {
        if (filter(unit) == false)
          continue;

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

      if (count.HasValue && restricted.Count < count)
      {
        return false;
      }

      if (order == null)
      {
        allocated = count == null 
          ? restricted 
          : restricted.Take(count.Value).ToList();
      }
      else
      {
        allocated = count == null
          ? restricted.OrderBy(order).ToList()
          : restricted.OrderBy(order).Take(count.Value).ToList();
      }            

      return true;
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