namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaCache
  {
    private readonly List<TrackableList<ManaUnit>> _groups;
    private readonly TrackableList<ManaUnit> _manaPool = new TrackableList<ManaUnit>();
    private readonly object _manaPoolCountLock = new object();
    private readonly TrackableList<ManaUnit> _removeList = new TrackableList<ManaUnit>();
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();

    public ManaCache()
    {
      _groups = new List<TrackableList<ManaUnit>>
        {
          new TrackableList<ManaUnit>(),
          new TrackableList<ManaUnit>(),
          new TrackableList<ManaUnit>(),
          new TrackableList<ManaUnit>(),
          new TrackableList<ManaUnit>(),
          _units,
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

    public void AddManaToPool(ManaAmount amount, ManaUsage usage)
    {
      lock (_manaPoolCountLock)
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
    }

    public int GetAvailableConvertedMana(ManaUsage usage, IEnumerable<ManaUnit> additional = null)
    {
      var restricted = new HashSet<ManaUnit>();
      var allocated = new List<ManaUnit>();

      additional = additional ?? Enumerable.Empty<ManaUnit>();

      var all = _units.Concat(additional).ToList();
      
      foreach (var manaUnit in all)
      {
        if (IsAvailable(manaUnit, restricted, usage))
        {
          restricted.Add(manaUnit);
          allocated.Add(manaUnit);

          RestrictUsingDifferentSourcesFromSameCard(manaUnit, restricted, all);
        }
      }

      return allocated.Count;
    }

    private void RestrictUsingDifferentSourcesFromSameCard(ManaUnit allocated, HashSet<ManaUnit> restricted, IEnumerable<ManaUnit> units)
    {
      // Same card cannot be tapped twice, therefore multiple sources 
      // from same card cannot be used simultaniously.

      // Add to restricted all units produced by another source
      // of the same card.
      var unitsProducedByAnotherSourceOnSameCard = units.Where(
        unit => unit.HasSource && allocated.HasSource &&
          unit.Source.OwningCard == allocated.Source.OwningCard
          && unit.Source != allocated.Source);

      foreach (var unit in unitsProducedByAnotherSourceOnSameCard)
      {
        restricted.Add(unit);
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
      {
        _units.Add(unit);
      }
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

    public bool Has(ManaAmount amount, ManaUsage usage, IEnumerable<ManaUnit> additional = null)
    {
      return TryToAllocateAmount(amount, usage, additional) != null;
    }

    public void Consume(ManaAmount amount, ManaUsage usage, IEnumerable<ManaUnit> additional = null)
    {
      var allocated = TryToAllocateAmount(amount, usage, additional);
      Asrt.True(allocated != null, "Not enough mana available.");

      var sources = GetSourcesToActivate(allocated);

      foreach (var source in sources)
      {
        lock (_manaPoolCountLock)
        {
          foreach (var unit in source.GetUnits())
          {
            _manaPool.Add(unit);
          }
        }

        source.PayActivationCost();
      }

      lock (_manaPoolCountLock)
      {
        foreach (var unit in allocated)
        {
          _manaPool.Remove(unit);
        }
      }

      foreach (var unit in allocated.Where(x => !x.HasSource))
      {
        RemovePermanently(unit);
      }
    }

    private void RemoveAllScheduled()
    {
      foreach (var unit in _removeList)
      {
        RemovePermanently(unit);
      }
      _removeList.Clear();
    }

    private void RemovePermanently(ManaUnit unit)
    {
      foreach (var colorIndex in unit.Color.Indices)
      {
        _groups[colorIndex].Remove(unit);
      }

      _units.Remove(unit);
    }

    private bool IsAvailable(ManaUnit unit, HashSet<ManaUnit> restricted, ManaUsage usage)
    {
      if (restricted.Contains(unit))
        return false;

      if (unit.CanActivateSource() == false && _manaPool.Contains(unit) == false)
        return false;

      if (unit.CanBeUsed(usage) == false)
        return false;

      return true;
    }

    private HashSet<ManaUnit> TryToAllocateAmount(ManaAmount amount, ManaUsage usage, IEnumerable<ManaUnit> additional)
    {
      var restricted = new HashSet<ManaUnit>();
      var allocated = new HashSet<ManaUnit>();

      var additionalGrouped = new[]
        {
          new List<ManaUnit>(),
          new List<ManaUnit>(),
          new List<ManaUnit>(),
          new List<ManaUnit>(),
          new List<ManaUnit>(),
          new List<ManaUnit>(),
        };

      IEnumerable<ManaUnit> allUnits = _units;
      var phyrexianUnits = new List<ManaUnit>();
      
      if (additional != null)
      {
        foreach (var unit in additional)
        {
          if (unit.Color.IsPhyrexian)
          {
            phyrexianUnits.Add(unit);
            continue;
          }

          foreach (var color in unit.Color.Indices)
          {
            additionalGrouped[color].Add(unit);
          }

          // Every mana can be used as colorless.
          // True colorless mana was already added, so we don't add it again.
          if (!unit.Color.IsColorless)
          {
            additionalGrouped[5].Add(unit);
          }
        }
        allUnits = _units.Concat(additional).ToList();
      }

      var checkAmount = amount
        .Select(x => new {Color = GetColorIndex(x), Count = x.Count, IsPhyrexian = x.Color.IsPhyrexian})
        .OrderBy(x => _groups[x.Color].Count)
        .ToArray();

      foreach (var manaOfSingleColor in checkAmount)
      {
        var ordered = _groups[manaOfSingleColor.Color]
          .Concat(additionalGrouped[manaOfSingleColor.Color])
          .OrderBy(GetManaUnitAllocationOrder)
          .ToList();

        for (var i = 0; i < manaOfSingleColor.Count; i++)
        {
          var allocatedUnit = ordered.FirstOrDefault(unit => IsAvailable(unit, restricted, usage));

          if (manaOfSingleColor.IsPhyrexian)
          {
            var phyrexianUnit = phyrexianUnits.FirstOrDefault(unit => IsAvailable(unit, restricted, usage));

            var totalOfSameColor = ordered.Count(x => x.Color.Indices.Any(index => index == manaOfSingleColor.Color));
            var totalOfAllColors = allUnits.Count(x => !x.Color.IsPhyrexian);

            // i reduces allocated units
            if (totalOfSameColor <= manaOfSingleColor.Count - i || totalOfAllColors < amount.Converted - i)
            {
              allocatedUnit = phyrexianUnit ?? allocatedUnit;
            }            
          }

          if (allocatedUnit == null)
            return null;

          restricted.Add(allocatedUnit);
          allocated.Add(allocatedUnit);

          if (!manaOfSingleColor.IsPhyrexian)
          {
            RestrictUsingDifferentSourcesFromSameCard(allocatedUnit, restricted, allUnits);
          }          
        }
      }

      return allocated;
    }

    private int GetColorIndex(SingleColorManaAmount manaOfSingleColor)
    {
      return manaOfSingleColor.Color.IsColorless
        ? 5
        // amount never contains mana whish is multiple colors at the same time
        : manaOfSingleColor.Color.Indices[0];
    }

    private int GetManaUnitAllocationOrder(ManaUnit x)
    {
      return _manaPool.Contains(x) ? 0 : x.Rank*10 + x.Color.Indices.Count;
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
  }
}