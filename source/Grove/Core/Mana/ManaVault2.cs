namespace Grove.Core.Mana
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  public class ManaAmount2 : IEnumerable<ManaColorCount2>
  {
    private readonly List<ManaColorCount2> _counts;

    public ManaAmount2(List<ManaColorCount2> counts)
    {
      _counts = counts;
    }

    public IEnumerator<ManaColorCount2> GetEnumerator()
    {
      return _counts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  public class ManaColorCount2
  {
    public ManaColor2 Color;
    public int Count;
  }

  public class ManaColor2
  {
    public const int White = 0;
    public const int Blue = 1;
    public const int Black = 2;
    public const int Red = 3;
    public const int Green = 4;

    private readonly List<int> _colorIndices = new List<int>(5);
    private readonly bool[] _isColor;

    public ManaColor2(bool isWhite = false, bool isBlue = false, bool isBlack = false, bool isRed = false,
      bool isGreen = false)
    {
      _isColor = new[] {isWhite, isBlue, isBlack, isRed, isGreen};

      for (var i = 0; i < _isColor.Length; i++)
      {
        if (_isColor[i])
        {
          _colorIndices.Add(i);
        }
      }
    }

    public List<int> Indices { get { return _colorIndices; } }    
  }

  public class ManaUnit2
  {
    public ManaUnit2(
      ManaColor2 color,
      int rank = 1,
      IManaSource2 source = null,
      object tapRestriction = null,
      int costRestriction = 0)
    {
      Color = color;
      Rank = rank;
      Source = source;
      TapRestriction = tapRestriction;
      CostRestriction = costRestriction;
    }

    public int CostRestriction { get; private set; }
    public IManaSource2 Source { get; private set; }
    public bool HasSource { get { return Source != null; } }
    public ManaColor2 Color { get; private set; }
    public int Rank { get; private set; }
    public object TapRestriction { get; private set; }

    public bool CanActivateSource()
    {
      if (Source == null)
        return false;

      return Source.CanActivate();
    }
  }

  public interface IManaSource2
  {
    bool CanActivate();
    List<ManaUnit2> Activate();
  }  

  public class ManaGroup2
  {
    private readonly LinkedList<ManaUnit2> _units = new LinkedList<ManaUnit2>();

    public void Add(ManaUnit2 unit)
    {
      if (_units.Count == 0)
      {
        _units.AddFirst(unit);
        return;
      }

      var node = _units.First;

      while (node != null)
      {
        if (node.Value.Rank >= unit.Rank)
        {
          _units.AddBefore(node, unit);
          return;
        }

        node = node.Next;
      }

      _units.AddLast(unit);
    }

    public void Remove(ManaUnit2 unit)
    {
      _units.Remove(unit);
    }

    public List<ManaUnit2> GetIf(int count, Func<ManaUnit2, bool> predicate)
    {
      var result = _units
        .Where(predicate)
        .Take(count)
        .ToList();

      return result.Count != count ? null : result;
    }
  }

  public class ManaVault2
  {
    private readonly ManaGroup2 _colorless = new ManaGroup2();
    private readonly ManaGroup2[] _groups;
    private readonly HashSet<ManaUnit2> _manaPool = new HashSet<ManaUnit2>();

    public ManaVault2()
    {
      _groups = new[]
        {
          new ManaGroup2(),
          new ManaGroup2(),
          new ManaGroup2(),
          new ManaGroup2(),
          new ManaGroup2(),
          _colorless,
        };
    }

    public void AddManaToPool(ManaAmount2 amount)
    {
      foreach (var mana in amount)
      {
        for (int i = 0; i < mana.Count; i++)
        {
          var unit = new ManaUnit2(mana.Color);          
          Register(unit);

          _manaPool.Add(unit);
        }
      }            
    }

    public void EmptyManaPool()
    {
      foreach (var unit in _manaPool.Where(x => !x.HasSource))
      {
        Unregister(unit);
      }
      
      _manaPool.Clear();
    }

    public void Register(ManaUnit2 unit)
    {
      foreach (var colorIndex in unit.Color.Indices)
      {
        _groups[colorIndex].Add(unit);
      }      

      _colorless.Add(unit);
    }

    public void Unregister(ManaUnit2 unit)
    {
      foreach (var colorIndex in unit.Color.Indices)
      {
        _groups[colorIndex].Remove(unit);
      }            

      _colorless.Remove(unit);
    }

    public bool Has(ManaAmount2 amount)
    {
      return TryToAllocateAmount(amount) != null;
    }

    private HashSet<ManaUnit2> TryToAllocateAmount(ManaAmount2 amount)
    {
      var tapRestrictions = new HashSet<object>();
      var allocated = new HashSet<ManaUnit2>();

      Func<ManaUnit2, bool> condition = x =>
        (x.CanActivateSource() || _manaPool.Contains(x)) &&
          (allocated.Contains(x) == false) &&
            (tapRestrictions.Contains(x.TapRestriction) == false);


      foreach (var mana in amount)
      {
        List<ManaUnit2> allocatable = null;
        
        foreach (var color in mana.Color.Indices)
        {
          allocatable = _groups[color].GetIf(mana.Count, condition);

          if (allocatable != null)
            break;
        }

        if (allocatable == null)
          return null;

        foreach (var unit in allocatable)
        {
          allocated.Add(unit);
        }
      }     

      var costRestriction = allocated.Sum(x => x.CostRestriction);

      if (costRestriction > 0)
      {
        var available = _colorless.GetIf(costRestriction,
          x => condition(x) && x.CostRestriction == 0);

        if (available == null)
          return null;

        foreach (var unit in available)
        {
          allocated.Add(unit);
        }
      }

      return allocated;
    }

    public void Consume(ManaAmount2 amount)
    {
      var allocated = TryToAllocateAmount(amount);
      Debug.Assert(allocated != null);

      var sources = GetSourcesToActivate(allocated);

      foreach (var source in sources)
      {
        var produced = source.Activate();

        foreach (var unit in produced)
        {
          _manaPool.Add(unit);
        }
      }

      foreach (var unit in allocated)
      {
        _manaPool.Remove(unit);
      }

      foreach (var unit in allocated.Where(x => !x.HasSource))
      {
        Unregister(unit);
      }
    }

    private List<IManaSource2> GetSourcesToActivate(IEnumerable<ManaUnit2> units)
    {
      return units
        .Where(x => x.HasSource)
        .GroupBy(x => x.Source)
        .Select(x => x.Key)
        .ToList();
    }
  }
}