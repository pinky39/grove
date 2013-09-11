namespace Grove.Gameplay.ManaHandling
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaVault
  {
    private readonly ManaUnits _colorless = new ManaUnits();
    private readonly List<ManaUnits> _groups;
    private readonly TrackableList<ManaUnit> _manaPool = new TrackableList<ManaUnit>();
    private readonly TrackableList<ManaUnit> _removeList = new TrackableList<ManaUnit>();
    private readonly object _manaPoolCountLock = new object();

    public ManaVault()
    {
      _groups = new List<ManaUnits>
        {
          new ManaUnits(),
          new ManaUnits(),
          new ManaUnits(),
          new ManaUnits(),
          new ManaUnits(),
          _colorless,
        };
    }

    public ManaCounts ManaPool
    {
      get
      {
        // this is accessed from a timer thread, which refreshes ui
        // if a call to empty mana pool is made at the same time 
        // the collection will be modified and an exception will be thrown,
        // a lock is needed to prevent this.
        lock (_manaPoolCountLock)
        {
          return new ManaCounts(
          white: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsWhite),
          blue: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsBlue),
          black: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsBlack),
          red: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsRed),
          green: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsGreen),
          multi: _manaPool.Count(x => x.Color.IsMulti),
          colorless: _manaPool.Count(x => x.Color.IsColorless)
          );  
        }                
      }
    }

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      foreach (var managroup in _groups)
      {
        managroup.Initialize(changeTracker);
      }

      _manaPool.Initialize(changeTracker);
      _removeList.Initialize(changeTracker);
    }

    public void AddManaToPool(IManaAmount amount, ManaUsage usage)
    {
      foreach (var mana in amount)
      {
        for (var i = 0; i < mana.Count; i++)
        {
          var unit = new ManaUnit(mana.Color, 0, usageRestriction: usage);
          Add(unit);

          _manaPool.Add(unit);
        }
      }
    }

    public IManaAmount GetAvailableMana(ManaUsage usage)
    {
      var counts = new Dictionary<ManaColor, int>();

      var restrictions = new Restrictions {Usage = usage};
      
      List<ManaUnit> available;      
      _colorless.TryToAllocate(x => IsAvailable(x, restrictions), out available);

      foreach (var unit in available)
      {
        if (!counts.ContainsKey(unit.Color))
        {
          counts[unit.Color] = 1;
        }
        else
        {
          counts[unit.Color]++;
        }
      }

      if (counts.Count == 1)
        return new SingleColorManaAmount(
          color: counts.Keys.First(),
          count: counts.Values.First());

      return new MultiColorManaAmount(counts);
    }

    public void AddManaToPool(IEnumerable<ManaUnit> units)
    {
      foreach (var unit in units)
      {
        _manaPool.Add(unit);
      }
    }

    public void EmptyManaPool()
    {
      foreach (var unit in _manaPool.Where(x => !x.HasSource))
      {
        RemovePermanently(unit);
      }
      
      lock (_manaPoolCountLock)
      {
        _manaPool.Clear();
      }

      RemoveAllScheduled();
    }

    public void Add(ManaUnit unit)
    {
      foreach (var colorIndex in unit.Color.Indices)
      {
        _groups[colorIndex].Add(unit);
      }

      // Every mana can be used as colorless.
      // True colorless mana was already added, so we don't add it again.
      if (unit.Color.IsColorless == false)
        _colorless.Add(unit);
    }

    public void Remove(ManaUnit unit)
    {
      if (_manaPool.Contains(unit))
      {
        _removeList.Add(unit);
        return;
      }

      RemovePermanently(unit);
    }

    private void RemoveAllScheduled()
    {
      foreach (var unit in _removeList)
      {
        RemovePermanently(unit);
      }
      _removeList.Clear();      
    }

    private void RemovePermanently(ManaUnit unit) {
      foreach (var colorIndex in unit.Color.Indices)
      {
        _groups[colorIndex].Remove(unit);
      }

      _colorless.Remove(unit);
    }

    public bool Has(IManaAmount amount, ManaUsage usage)
    {
      return TryToAllocateAmount(amount, usage) != null;
    }

    private bool IsAvailable(ManaUnit unit, Restrictions restrictions)
    {                  
      if (restrictions.Allocated.Contains(unit))
        return false;      
      
      if (unit.CanActivateSource() == false && _manaPool.Contains(unit) == false)
        return false;

      if (unit.CanBeUsed(restrictions.Usage) == false)
        return false;

      if (unit.TapRestriction != null)
      {
        IManaSource source;
        if (restrictions.Tap.TryGetValue(unit.TapRestriction, out source) && source != unit.Source)
        {
          return false;
        }
      }

      if (unit.SacRestriction != null)
      {
        IManaSource source;
        if (restrictions.Sac.TryGetValue(unit.SacRestriction, out source) && source != unit.Source)
        {
          return false;
        }
      }

      return true;
    }

    private HashSet<ManaUnit> TryToAllocateAmount(IManaAmount amount, ManaUsage usage)
    {
      var restrictions = new Restrictions {Usage = usage};

      Func<ManaUnit, int> ordering = x => _manaPool.Contains(x) ? 0 : x.Rank*10 + x.Color.Indices.Count;

      foreach (var colorMana in amount)
      {
        List<ManaUnit> allocated = null;

        foreach (var color in colorMana.Color.Indices)
        {                    
          if ( _groups[color].TryToAllocate(
            filter: x => IsAvailable(x, restrictions), 
            allocated: out allocated, 
            count: colorMana.Count, 
            order: ordering))
          {
            break;
          }         
        }

        if (allocated == null)
          return null;

        foreach (var unit in allocated)
        {
          restrictions.Allocated.Add(unit);

          if (unit.TapRestriction != null && !restrictions.Tap.ContainsKey(unit.TapRestriction))
            restrictions.Tap.Add(unit.TapRestriction, unit.Source);

          if (unit.SacRestriction != null && !restrictions.Sac.ContainsKey(unit.SacRestriction))
            restrictions.Sac.Add(unit.SacRestriction, unit.Source);
        }
      }

      var costRestriction = restrictions.Allocated.Sum(x => x.ActivationCostRestriction);

      if (costRestriction > 0)
      {
        List<ManaUnit> allocated = null;
        
        if (
        _colorless.TryToAllocate(
          filter: x => x.ActivationCostRestriction == 0 && IsAvailable(x, restrictions), 
          allocated: out allocated, 
          count: costRestriction, 
          order: ordering) == false)

        {
          return null;
        }        
                
        foreach (var unit in allocated)
        {
          restrictions.Allocated.Add(unit);
        }
      }

      return restrictions.Allocated;
    }

    public void Consume(IManaAmount amount, ManaUsage usage)
    {
      var allocated = TryToAllocateAmount(amount, usage);
      AssertEx.True(allocated != null, "Not enough mana available.");      
      
      var sources = GetSourcesToActivate(allocated);

      foreach (var source in sources)
      {
        foreach (var unit in source.GetUnits())
        {
          _manaPool.Add(unit);
        }
        
        source.PayActivationCost();        
      }

      foreach (var unit in allocated)
      {
        _manaPool.Remove(unit);
      }

      foreach (var unit in allocated.Where(x => !x.HasSource))
      {
        RemovePermanently(unit);
      }
    }

    private List<IManaSource> GetSourcesToActivate(IEnumerable<ManaUnit> units)
    {
      return units
        .Where(x => x.HasSource)
        .Where(x => !_manaPool.Contains(x))
        .GroupBy(x => x.Source)
        .Select(x => x.Key)
        .ToList();
    }

    private class Restrictions
    {
      public readonly HashSet<ManaUnit> Allocated = new HashSet<ManaUnit>();
      public readonly Dictionary<object, IManaSource> Tap = new Dictionary<object, IManaSource>();      
      public readonly Dictionary<object, IManaSource> Sac = new Dictionary<object, IManaSource>();      
      public ManaUsage Usage;
    }
  }
}