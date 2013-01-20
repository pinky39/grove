namespace Grove.Core
{
  using Triggers;

  public class TriggeredAbilityParameters : AbilityParameters
  {
    public bool TriggerOnlyIfOwningCardIsInPlay;
    public Trigger[] Triggers;
  }
}