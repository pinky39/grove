﻿namespace Grove.Gameplay.Zones
{
  using System.Linq;
  using Card;
  using Infrastructure;
  using Player;

  public class Library : OrderedZone, ILibraryQuery
  {
    public Library(Player owner) : base(owner) {}

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