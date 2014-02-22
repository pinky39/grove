namespace Grove.Gameplay.Triggers
{
  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility, Game game);
  }
}