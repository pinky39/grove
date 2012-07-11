namespace Grove.Core.Zones
{
  using Infrastructure;

  public class Graveyard : OrderedZone
  {
    public Graveyard(ChangeTracker changeTracker) : base(changeTracker) {}

    private Graveyard()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Graveyard; } }
  }
}