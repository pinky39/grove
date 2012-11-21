namespace Grove.Core.Zones
{
  using System;

  public class Exile : UnorderedZone
  {
    public Exile(Player owner, Game game) : base(owner, game) {}

    private Exile()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Exile; } }
  }
}