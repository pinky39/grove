namespace Grove.Core.Decisions.Results
{
  public class Spell : Playable
  {
    private Spell() {}

    public Spell(Card card, ActivationParameters activationParameters, int index)
    {
      Card = card;
      ActivationParameters = activationParameters;
      Index = index;
    }

    protected ActivationParameters ActivationParameters { get; private set; }
    protected int Index { get; private set; }
    protected Card Card { get; private set; }

    public override void Play()
    {
      Card.Cast(Index, ActivationParameters);
    }

    public override string ToString()
    {
      return string.Format("spell {0} of {1}",Index, Card);
    }
  }
}