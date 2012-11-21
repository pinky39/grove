namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;

  public class Graveyard : OrderedZone, IGraveyardQuery
  {
    public Graveyard(Player owner, Game game) : base(owner, game) {}

    private Graveyard()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Graveyard; } }
    public int Score { get { return this.Sum(x => x.Score); } }
    public IEnumerable<Card> Creatures { get { return this.Where(x => x.Is().Creature); } }
  }
}