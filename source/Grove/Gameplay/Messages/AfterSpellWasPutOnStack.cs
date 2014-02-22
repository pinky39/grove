namespace Grove.Gameplay.Messages
{
  using System.Collections.Generic;

  public class AfterSpellWasPutOnStack : SpellWasCast
  {
    public AfterSpellWasPutOnStack(Card card, IEnumerable<ITarget> targets) : base(card, targets) {}
  }
}