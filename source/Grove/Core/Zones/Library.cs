namespace Grove.Core.Zones
{
  using System.Linq;
  using Infrastructure;
  using Messages;

  public class Library : OrderedZone, ILibraryQuery
  {
    public Library(Player owner, Game game) : base(owner, game) {}

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