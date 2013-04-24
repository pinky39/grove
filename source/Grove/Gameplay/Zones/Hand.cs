﻿namespace Grove.Gameplay.Zones
{
  using System.Linq;
  using Player;

  public class Hand : UnorderedZone, IHandQuery
  {
    public Hand(Player owner) : base(owner) {}

    private Hand()
    {
      /* for state copy */
    }

    public bool CanMulligan { get { return Count >= 1; } }
    public int MulliganSize { get { return Count - 1; } }

    public int Score { get { return this.Sum(x => x.Score); } }
    public override Zone Zone { get { return Zone.Hand; } }
  }
}