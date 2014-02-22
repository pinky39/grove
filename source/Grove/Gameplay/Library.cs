namespace Grove.Gameplay
{
  using System.Linq;
  using Grove.Infrastructure;

  public class Library : OrderedZone, ILibraryQuery
  {
    public Library(Player owner) : base(owner) {}

    private Library()
    {
      /* for state copy */
    }

    public override Zone Name { get { return Zone.Library; } }
    public Card Top { get { return this.FirstOrDefault(); } }

    public override int CalculateHash(HashCalculator calc)
    {
      return Count;
    }

    public void PutOnTop(Card card)
    {
      if (card.Zone == Name)
      {
        MoveToFront(card);
      }
      else
      {
        AddToFront(card);
      }
    }   
 
    public void PutOnBottom(Card card)
    {
      if (card.Zone == Name)
      {
        MoveToEnd(card);
      }
      else
      {
        AddToEnd(card);
      }
    }
  }
}