﻿namespace Grove.Gameplay.Zones
{
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Player;

  public class Graveyard : OrderedZone, IGraveyardQuery
  {
    public Graveyard(Player owner) : base(owner) {}

    private Graveyard()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Graveyard; } }
    public int Score { get { return this.Sum(x => x.Score); } }
    public IEnumerable<Card> Creatures { get { return this.Where(x => x.Is().Creature); } }
  }
}