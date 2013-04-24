namespace Grove.Gameplay.Card.Triggers
{
  using Abilities;

  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility, Game game);
  }
}