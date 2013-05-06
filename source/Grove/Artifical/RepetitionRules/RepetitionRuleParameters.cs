namespace Grove.Artifical.RepetitionRules
{
  using Gameplay.Targeting;

  public class RepetitionRuleParameters
  {
    public RepetitionRuleParameters(int maxRepetitions, Targets targets = null)
    {
      MaxRepetitions = maxRepetitions;
      Targets = targets;
    }

    public int MaxRepetitions { get; private set; }
    public Targets Targets { get; private set; }
  }
}