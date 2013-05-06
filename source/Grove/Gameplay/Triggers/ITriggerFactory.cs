namespace Grove.Gameplay.Triggers
{
  using Abilities;

  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility, Game game);
  }
}