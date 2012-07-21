namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class Library : OrderedZone
  {
    public Library(Game game) : base(game) {}

    private Library()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Library; } }
    public Card Top { get { return this.FirstOrDefault(); } }       

    public override int CalculateHash(HashCalculator calc)
    {
      return Count;
    }   

    public void PutOnTop(Card card)
    {
      AddToFront(card);
    }
  }
}