namespace Grove.Core.Decisions.Results
{
  using Cards;

  public class Ability : Playable
  {
    private Ability() {}

    public Ability(Card card, ActivationParameters activationParameters, int index)
    {
      Card = card;
      ActivationParameters = activationParameters;
      Index = index;
    }

    protected ActivationParameters ActivationParameters { get; private set; }
    protected Card Card { get; private set; }
    protected int Index { get; private set; }

    public override void Play()
    {
      Card.ActivateAbility(Index, ActivationParameters);
    }

    public override string ToString()
    {
      return string.Format("ability {0} of {1}", Index, Card);
    }
  }
}