namespace Grove.Core.Controllers.Results
{
  public class Cyclable : Playable
  {
    private Cyclable() {}

    public Cyclable(Card card)
    {
      Card = card;
    }

    protected Card Card { get; private set; }

    public override void Play()
    {
      Card.Controller.CycleCard(Card);
    }

    public override string ToString()
    {
      return string.Format("cycling {0}", Card);
    }
  }
}