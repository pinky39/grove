namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;

  public class Graveyard : OrderedZone, IGraveyardQuery
  {
    public Graveyard(Game game) : base(game) {}

    private Graveyard()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Graveyard; } }
    public IEnumerable<Card> Creatures { get { return this.Where(x => x.Is().Creature); } }
  }
}