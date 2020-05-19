namespace Grove
{
  using System.Linq;
  using System.Security.Policy;
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
    public Card Bottom { get { return this.LastOrDefault(); } }

    public override int CalculateHash(HashCalculator calc)
    {      
      var visible = this
        .Where(x => x.IsVisibleToPlayer(Owner))
        .ToList();

      if (visible.Count == 0)
        return Count;
      
      return HashCalculator.Combine(Count, calc.Calculate(visible, true));
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