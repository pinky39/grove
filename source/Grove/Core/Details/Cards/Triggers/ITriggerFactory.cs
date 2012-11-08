namespace Grove.Core.Details.Cards.Triggers
{
  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility, Game game);
  }
}