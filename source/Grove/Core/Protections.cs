namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Protections : IModifiable, IHashable
  {
    private readonly TrackableList<ManaColors> _colors;
    private readonly TrackableList<string> _cardTypes;

    private Protections() {}

    public Protections(ChangeTracker changeTracker, IHashDependancy hashDependancy, ManaColors manaColors = ManaColors.None, string[] cardTypes = null)
    {
      if (manaColors == ManaColors.None)
      {
        _colors = new TrackableList<ManaColors>(changeTracker, hashDependancy);        
      }
      else
      {
        _colors = new TrackableList<ManaColors>(manaColors.ToEnumerable(),
          changeTracker, hashDependancy);  
      }

      if (cardTypes == null)
      {
        _cardTypes = new TrackableList<string>(changeTracker, hashDependancy);
      }
      else
      {
        _cardTypes = new TrackableList<string>(cardTypes, changeTracker, hashDependancy);
      }            
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_colors);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AddProtectionFromColors(ManaColors color)
    {
      _colors.Add(color);
    }    

    public bool HasProtectionFrom(CardType type)
    {
      return type.IsAny(_cardTypes);
    }

    public bool HasProtectionFrom(ManaColors colors)
    {
      var mergedProtection = _colors.Aggregate(ManaColors.None,
        (current, protectionFromColors) => current | protectionFromColors);

      return (colors & mergedProtection) != ManaColors.None;
    }

    public void RemoveProtectionFromColors(ManaColors colors)
    {
      _colors.Remove(colors);
    }
  }
}