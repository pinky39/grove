namespace Grove.Core.Triggers
{
  public interface ITriggerFactory
  {
    Trigger CreateTrigger(TriggeredAbility triggeredAbility);
  }
}