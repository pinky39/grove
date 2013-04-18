namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class CardColors : Characteristic<List<CardColor>>, IEnumerable<CardColor>, IModifiable, IHashable
  {
    private CardColors() {}

    public CardColors(IEnumerable<CardColor> colors) : base(colors.ToList()) {}

    public int Count { get { return Value.Count; } }

    public IEnumerator<CardColor> GetEnumerator()
    {
      return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public int CalculateHash(HashCalculator calc)
    {
      var hashcodes = new List<int>();

      foreach (var item in Value)
      {        
        hashcodes.Add(item.GetHashCode());
      }
      
      return HashCalculator.CombineCommutative(hashcodes);
    }
  }
}