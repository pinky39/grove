namespace Grove.Core.Zones
{
  public class Exile : UnorderedZone
  {
    public Exile(Game game) : base(game) {}

    private Exile()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Exile; } }
  }
}