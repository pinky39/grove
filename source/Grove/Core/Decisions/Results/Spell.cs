namespace Grove.Core.Decisions.Results
{
  using Cards;

  public class Spell : Playable
  {
    private Spell() {}

    public Spell(Card card, ActivationParameters activationParameters)
    {
      Card = card;
      ActivationParameters = activationParameters;
    }

    protected ActivationParameters ActivationParameters { get; private set; }
    protected Card Card { get; private set; }

    public override void Play()
    {
      Card.Cast(ActivationParameters);      
    }

    public override string ToString()
    {
      return string.Format("spell {0}", Card);
    }
  }
}