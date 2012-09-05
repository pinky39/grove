namespace Grove.Core.Zones
{
  using System.Linq;

  public class Hand : UnorderedZone, IHandQuery
  {
    public Hand(Game game) : base(game) {}

    private Hand()
    {
      /* for state copy */
    }

    public bool CanMulligan { get { return Count >= 1; } }
    public int MulliganSize { get { return Count - 1; } }

    public int Score { get { return this.Sum(x => x.Score); } }
    public override Zone Zone { get { return Zone.Hand; } }

    public override void AfterRemove(Card card)
    {            
      card.IsRevealed = false;
    }
  }
}