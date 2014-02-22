namespace Grove.Gameplay.AI.RepetitionRules
{
  public class RepetitionRuleParameters
  {
    public RepetitionRuleParameters(Card card, int maxRepetitions, Targets targets = null)
    {
      Card = card;
      MaxRepetitions = maxRepetitions;
      Targets = targets;
    }

    public Card Card { get; private set; }
    public int MaxRepetitions { get; private set; }
    public Targets Targets { get; private set; }
  }
}