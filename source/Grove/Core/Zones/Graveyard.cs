namespace Grove.Core.Zones
{
  public class Graveyard : OrderedZone
  {
    
    public Graveyard(Game game) : base(game)
    {      
    }

    private Graveyard()
    {
      /* for state copy */
    }

    public override Zone Zone { get { return Zone.Graveyard; } }    
  }
}