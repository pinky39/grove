namespace Grove.Core.Mana
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Text.RegularExpressions;
  using Infrastructure;

  public class ManaAmount2 : IEnumerable<ManaColorCount2>
  {
    private readonly List<ManaColorCount2> _counts;

    public ManaAmount2(List<ManaColorCount2> counts)
    {
      _counts = counts;
    }

    public ManaAmount2(string amount)
    {
      _counts = ParseMana(amount);
    }

    private static List<ManaColorCount2> ParseMana(string str)
    {
      str = str.ToLowerInvariant();
      var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);
      var parsed = new List<ManaColorCount2>();
      ManaColorCount2 current = null;
            
      foreach (var token in tokens)
      {
        var colorless = ParseColorless(token);

        if (colorless.HasValue)
        {          
          parsed.Add(new ManaColorCount2(ManaColor2.Colorless, colorless.Value));          
          continue;
        }

        var color = ParseColored(token);

        if (current == null)
        {
          current = new ManaColorCount2(color, 1);
        }
        else if (current.Color != color)
        {
          parsed.Add(current);
          current = new ManaColorCount2(color, 1);
        }
        else
        {
          current.Count++;
        }        
      }

      if (current != null)
        parsed.Add(current);

      return parsed;
    }

    private static ManaColor2 ParseColored(string token)
    {
      bool isWhite = false, isBlue = false, isBlack = false, isRed = false, isGreen = false;

      foreach (var ch in token)
      {
        switch (Char.ToUpper(ch))
        {
            case ('W'):
            isWhite = true;
            break;
            case ('U'):
            isBlue = true;
            break;
            case ('B'):
            isBlack = true;
            break;
            case ('R'):
            isRed = true;
            break;
            case ('G'):
            isGreen = true;
            break;
        }
      }
      
      return new ManaColor2(isWhite, isBlue, isBlack, isRed, isGreen);
    }

    private static int? ParseColorless(string token)
    {
      int count;
      if (Int32.TryParse(token, out count))
        return count;

      return null;
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
    public ManaColorCount2(ManaColor2 color, int count)
    {
      Color = color;
      Count = count;
    }

    public ManaColor2 Color { get; private set; }
    public int Count { get; set; }
  }

  public class ManaColor2 : IEquatable<ManaColor2>
  {
    public static readonly ManaColor2 White = new ManaColor2(isWhite: true);
    public static readonly ManaColor2 Blue = new ManaColor2(isBlue: true);
    public static readonly ManaColor2 Black = new ManaColor2(isBlack: true);
    public static readonly ManaColor2 Red = new ManaColor2(isRed: true);
    public static readonly ManaColor2 Green = new ManaColor2(isGreen: true);
    public static readonly ManaColor2 Colorless = new ManaColor2(isColorless: true);
    
    private readonly List<int> _colorIndices = new List<int>(6);
    private readonly bool[] _isColor;

    public ManaColor2(bool isWhite = false, bool isBlue = false, bool isBlack = false, bool isRed = false,
      bool isGreen = false, bool isColorless = false)
    {
      _isColor = new[] {isWhite, isBlue, isBlack, isRed, isGreen, isColorless};

      for (var i = 0; i < _isColor.Length; i++)
      {
        if (_isColor[i])
        {
          _colorIndices.Add(i);
        }
      }
    }

    public List<int> Indices { get { return _colorIndices; } }

    public bool Equals(ManaColor2 other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      return _colorIndices.SequenceEqual(other._colorIndices);            
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (ManaColor2)) return false;
      return Equals((ManaColor2) obj);
    }

    public override int GetHashCode()
    {
      return _colorIndices.GetHashCode();
    }

    public static bool operator ==(ManaColor2 left, ManaColor2 right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ManaColor2 left, ManaColor2 right)
    {
      return !Equals(left, right);
    }
  }

  public class ManaUnit2
  {
    public ManaUnit2(
      ManaColor2 color,
      int rank = 1,      
      IManaSource2 source = null,
      object tapRestriction = null,
      int costRestriction = 0,
      ManaUsage usageRestriction = ManaUsage.Any)
    {
      Color = color;
      Rank = rank;
      Source = source;
      TapRestriction = tapRestriction;
      CostRestriction = costRestriction;
      UsageRestriction = usageRestriction;
    }

    public int CostRestriction { get; private set; }
    public ManaUsage UsageRestriction { get; private set; }
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
    private readonly TrackableList<ManaUnit2> _units = new TrackableList<ManaUnit2>();

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      _units.Initialize(changeTracker);
    }
    
    public void Add(ManaUnit2 unit)
    {
      if (_units.Count == 0)
      {
        _units.Add(unit);
        return;
      }

      for (int i = 0; i < _units.Count; i++)
      {
        if (_units[i].Rank >= unit.Rank)
        {
          _units.Insert(i, unit);
          return;
        }
      }

      _units.Add(unit);            
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
    private readonly TrackableList<ManaUnit2> _manaPool = new TrackableList<ManaUnit2>();

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

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      foreach (var managroup in _groups)
      {
        managroup.Initialize(changeTracker);
      }

      _manaPool.Initialize(changeTracker);
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

    public bool Has(ManaAmount2 amount, ManaUsage usage)
    {
      return TryToAllocateAmount(amount, usage) != null;
    }

    private HashSet<ManaUnit2> TryToAllocateAmount(ManaAmount2 amount, ManaUsage usage)
    {
      var tapRestrictions = new HashSet<object>();
      var allocated = new HashSet<ManaUnit2>();

      Func<ManaUnit2, bool> condition = x =>
        (x.UsageRestriction == ManaUsage.Any || x.UsageRestriction == usage) &&
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

    public void Consume(ManaAmount2 amount, ManaUsage usage)
    {
      var allocated = TryToAllocateAmount(amount, usage);
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