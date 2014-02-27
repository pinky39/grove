namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Modifiers;

  public class CardColors : Characteristic<List<CardColor>>, IEnumerable<CardColor>, IAcceptsCardModifier, IHashable
  {
    private CardColors() { }

    public CardColors(IEnumerable<CardColor> colors) : base(colors.ToList()) { }

    public int Count { get { return Value.Count; } }
    public void Accept(ICardModifier modifier) { modifier.Apply(this); }
    public IEnumerator<CardColor> GetEnumerator() { return Value.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public int CalculateHash(HashCalculator calc)
    {
      var hashcodes = Value.Select(item => item.GetHashCode())
        .ToList();

      return HashCalculator.CombineCommutative(hashcodes);
    }
  }
}