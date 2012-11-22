namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;

  public class Hand : UnorderedZone, IHandQuery
  {
    public Hand(Player owner, Game game) : base(owner, game) {}

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

    public override IEnumerable<Card> GenerateTargets(System.Func<Zone, Player, bool> zoneFilter)
    {
      return base.GenerateTargets(zoneFilter).Where(x => !x.IsHidden);
    }
  }
}