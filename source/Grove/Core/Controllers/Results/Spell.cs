namespace Grove.Core.Controllers.Results
{
  public class Spell : Playable
  {
    protected ActivationParameters ActivationParameters { get; private set; }
    protected Card Card { get; private set; }

    private Spell() {}
    
    public Spell(Card card, ActivationParameters activationParameters)
    {
      Card = card;
      ActivationParameters = activationParameters;
    }

    public override void Play()
    {
      Card.Controller.CastSpell(Card, ActivationParameters);
    }                
  }
}