namespace Grove.Gameplay.Zones
{
  using System;

  [Serializable]
  public class Exile : UnorderedZone
  {
    public Exile(Player owner) : base(owner) {}

    private Exile()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Exile; } }
  }
}