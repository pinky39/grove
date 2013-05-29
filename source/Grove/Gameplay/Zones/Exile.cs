namespace Grove.Gameplay.Zones
{
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