namespace Grove.Core.Messages
{
  public class AssignedCombatDamageWasDealt
  {
    public Step Step { get; private set; }

    public AssignedCombatDamageWasDealt(Step step)
    {
      Step = step;
    }
  }
}