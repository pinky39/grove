namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using Grove.Infrastructure;
  using Grove.Gameplay.Modifiers;

  [Copyable]
  public class Protections : IAcceptsCardModifier, IHashable
  {
    private readonly TrackableList<string> _cardTypes = new TrackableList<string>();
    private readonly TrackableList<CardColor> _colors = new TrackableList<CardColor>();

    private Protections() {}

    public Protections(IEnumerable<CardColor> colors, IEnumerable<string> cardTypes)
    {
      _colors = new TrackableList<CardColor>(colors);
      _cardTypes = new TrackableList<string>(cardTypes);
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(calc.Calculate(_colors), calc.Calculate(_cardTypes));
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _cardTypes.Initialize(changeTracker, hashDependancy);
      _colors.Initialize(changeTracker, hashDependancy);
    }

    public bool HasProtectionFromAnyColor()
    {
      return _colors.Count > 0;
    }

    public bool HasProtectionFromAnyTypes()
    {
      return _cardTypes.Count > 0;
    }

    public void AddProtectionFromColor(CardColor color)
    {
      _colors.Add(color);
    }

    public void AddProtectionFromCards(params string[] cardTypes)
    {
      _cardTypes.AddRange(cardTypes);
    }

    public bool HasProtectionFrom(string type)
    {
      return _cardTypes.Contains(type.ToLowerInvariant());
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