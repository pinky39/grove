namespace Grove.Core
{
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Attachment : IHashable
  {
    private readonly List<Modifier> _modifiers = new List<Modifier>();

    public Attachment(Card card, List<Modifier> modifiers)
    {
      Card = card;
      _modifiers.AddRange(modifiers);
    }

    private Attachment() {}

    public Card Card { get; private set; }

    public IEnumerable<Modifier> Modifiers { get { return _modifiers; } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(Card);
    }
  }
}