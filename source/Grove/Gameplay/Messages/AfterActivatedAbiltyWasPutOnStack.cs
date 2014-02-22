namespace Grove.Gameplay.Messages
{
  using System.Collections.Generic;

  public class AfterActivatedAbiltyWasPutOnStack : AbilityActivated
  {
    public AfterActivatedAbiltyWasPutOnStack(ActivatedAbility ability, IEnumerable<ITarget> targets)
      : base(ability, targets) {}
  }
}