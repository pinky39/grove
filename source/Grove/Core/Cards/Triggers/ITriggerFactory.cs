namespace Grove.Core.Cards.Triggers
{
  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility, Game game);
  }
}