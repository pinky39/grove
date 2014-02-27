namespace Grove.Events
{
  using System;
  using System.Collections.Generic;

  public class BeforeActivatedAbilityWasPutOnStack : AbilityActivated, ICardActivationMessage
  {
    public BeforeActivatedAbilityWasPutOnStack(ActivatedAbility ability, IEnumerable<ITarget> targets)
      : base(ability, targets) {}

    public string GetTitle()
    {
      return String.Format("{0} activates...", Controller);
    }
  }
}