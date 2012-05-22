namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class Library : OrderedZone
  {
    public Library(IEnumerable<Card> cards, ChangeTracker changeTracker) : base(cards, changeTracker)
    {     
    }

    private Library()
    {
      /* for state copy */
    }

    public override Zone Zone
    {
      get { return Zone.Library; }
    }

    public Card Draw()
    {
      var top = this.FirstOrDefault();

      if (top == null)
        return null;

      Remove(top);      

      return top;
    }

    public override int CalculateHash(HashCalculator hashCalculator)
    {
      return Count;
    }

    public void Shuffle(IEnumerable<Card> cards)
    {
      foreach (var card in cards)
      {
        Add(card);
      }

      Shuffle();
    }
  }
}