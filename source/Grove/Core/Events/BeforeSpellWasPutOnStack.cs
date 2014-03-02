namespace Grove.Events
{
  using System;
  using System.Collections.Generic;
  using Effects;

  public class BeforeSpellWasPutOnStack : SpellWasCast, ICardActivationMessage
  {
    public BeforeSpellWasPutOnStack(Card card, IEnumerable<ITarget> targets) : base(card, targets) {}
    
    public string GetTitle()
    {
      return String.Format("{0} casts...", Controller);
    }
  }
}