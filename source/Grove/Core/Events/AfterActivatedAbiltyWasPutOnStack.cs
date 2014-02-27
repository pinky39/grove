namespace Grove.Events
{
  using System.Collections.Generic;

  public class AfterActivatedAbiltyWasPutOnStack : AbilityActivated
  {
    public AfterActivatedAbiltyWasPutOnStack(ActivatedAbility ability, IEnumerable<ITarget> targets)
      : base(ability, targets) {}
  }
}