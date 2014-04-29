namespace Grove.Events
{
  public class ActivatedAbilityPutOnStackEvent : AbilityActivatedEvent
  {
    public ActivatedAbilityPutOnStackEvent(ActivatedAbility ability, Targets targets)
      : base(ability, targets) {}
  }
}