namespace Grove.Gameplay.Triggers
{
  using System;
  using Abilities;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class OnEffectResolved : Trigger, IReceive<EffectResolved>
  {
    private readonly Func<TriggeredAbility, Game, bool> _filter;

    private OnEffectResolved() {}

    public OnEffectResolved(Func<TriggeredAbility, Game, bool> filter)
    {
      _filter = filter;
    }

    public void Receive(EffectResolved message)
    {
      if (!_filter(Ability, Game))
        return;

      Set(message);
    }
  }
}