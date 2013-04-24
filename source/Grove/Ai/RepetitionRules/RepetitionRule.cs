namespace Grove.Ai.RepetitionRules
{
  public abstract class RepetitionRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {
      var p = new RepetitionRuleParameters(c);
      c.Repeat = GetRepetitionCount(p);
    }

    public abstract int GetRepetitionCount(RepetitionRuleParameters p);
  }
}