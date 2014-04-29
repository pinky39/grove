namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnEffectResolved : Trigger, IReceive<EffectResolvedEvent>
  {
    private readonly Func<TriggeredAbility, Game, bool> _filter;

    private OnEffectResolved() {}

    public OnEffectResolved(Func<TriggeredAbility, Game, bool> filter)
    {
      _filter = filter;
    }

    public void Receive(EffectResolvedEvent message)
    {
      if (!_filter(Ability, Game))
        return;

      Set(message);
    }
  }
}