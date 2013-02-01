namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Modifiers;

  [Copyable]
  public class Protections : IModifiable, IHashable
  {
    private readonly TrackableList<string> _cardTypes = new TrackableList<string>();
    private readonly TrackableList<ManaColors> _colors = new TrackableList<ManaColors>();
  
    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_colors);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _cardTypes.Initialize(changeTracker, hashDependancy);
      _colors.Initialize(changeTracker, hashDependancy);
    }

    public void AddProtectionFromColors(ManaColors color)
    {
      _colors.Add(color);
    }

    public void AddProtectionFromCards(params string[] cardTypes)
    {
      _cardTypes.AddRange(cardTypes);
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