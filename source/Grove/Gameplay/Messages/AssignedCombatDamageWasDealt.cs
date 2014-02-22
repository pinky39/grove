namespace Grove.Gameplay.Messages
{
  public class AssignedCombatDamageWasDealt
  {
    public AssignedCombatDamageWasDealt(Step step)
    {
      Step = step;
    }

    public Step Step { get; private set; }
  }
}