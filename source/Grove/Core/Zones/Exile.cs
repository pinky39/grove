namespace Grove
{
  public class Exile : UnorderedZone
  {
    public Exile(Player owner) : base(owner) {}

    private Exile()
    {
      /* for state copy */
    }

    public override Zone Name { get { return Zone.Exile; } }
  }
}