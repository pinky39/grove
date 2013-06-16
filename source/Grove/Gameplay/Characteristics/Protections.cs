namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Protections : IAcceptsCardModifier, IHashable
  {
    private readonly TrackableList<string> _cardTypes = new TrackableList<string>();
    private readonly TrackableList<CardColor> _colors = new TrackableList<CardColor>();

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(calc.Calculate(_colors), calc.Calculate(_cardTypes));
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _cardTypes.Initialize(changeTracker, hashDependancy);
      _colors.Initialize(changeTracker, hashDependancy);
    }

    public void AddProtectionFromColor(CardColor color)
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

    public bool HasProtectionFrom(CardColor color)
    {
      return _colors.Contains(color);
    }

    public void RemoveProtectionFromColor(CardColor color)
    {
      _colors.Remove(color);
    }
  }
}