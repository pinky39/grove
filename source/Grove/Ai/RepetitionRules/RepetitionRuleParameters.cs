namespace Grove.Core.Ai.RepetitionRules
{
  public class RepetitionRuleParameters
  {
    public RepetitionRuleParameters(Ai.ActivationContext context)
    {
      MaxRepetitions = context.MaxRepetitions;
    }

    public int MaxRepetitions { get; private set; }
  }
}