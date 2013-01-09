namespace Grove.Core.Cards.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnEffectResolved : Trigger, IReceive<EffectResolved>
  {
    public Func<TriggeredAbility, Game, bool> Filter =
      delegate { return true; };
    
    public void Receive(EffectResolved message)
    {
      if (!Filter(Ability, Game))
        return;

      Set(message);
    }
  }
}