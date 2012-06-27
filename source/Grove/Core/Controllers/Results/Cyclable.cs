namespace Grove.Core.Controllers.Results
{
  public class Cyclable : Playable
  {
    private Cyclable()
    {      
    }
    
    protected Card Card { get; private set; }

    public Cyclable(Card card)
    {
      Card = card;      
    }

    public override void Play()
    {
      Card.Controller.CycleSpell(Card);
    }

    public override string ToString()
    {
      return string.Format("cycling {0}", Card);
    }
  }
}