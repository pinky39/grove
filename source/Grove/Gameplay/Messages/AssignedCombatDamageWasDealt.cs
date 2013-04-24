namespace Grove.Gameplay.Messages
{
  using States;

  public class AssignedCombatDamageWasDealt
  {
    public Step Step { get; private set; }

    public AssignedCombatDamageWasDealt(Step step)
    {
      Step = step;
    }
  }
}