namespace Grove.Core.Triggers
{
  using System;
  using Grove.Infrastructure;
  using Grove.Core.Messages;

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