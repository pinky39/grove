namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnZoneChanged : Trigger, IReceive<ZoneChangedEvent>
  {
    private readonly CardSelector _selector;
    private readonly Zone _from;
    private readonly Zone _to;

    private OnZoneChanged() {}

    public Zone To { get { return _to; } }

    public OnZoneChanged(Zone @from = Zone.None, Zone to = Zone.None,
      CardSelector selector = null)
    {
      _from = from;
      _to = to;
      _selector = selector ?? ((card, ctx) => ctx.OwningCard == card);
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (!_selector(message.Card, new Context(this, Game)))
        return;

      // all scenario cards have zone set to None
      // to avoid triggering abiltiies when 
      // scenario is initialized
      if (message.From == Zone.None)
        return;

      if (message.ToBattlefield &&
        message.Card.Is().Creature &&
        Game.Has().CreaturesEnteringBattlefieldDontTriggerAbilities)
      {
        return;
      }

      if (_from == Zone.None && _to == message.To)
        Set(message);
      else if (_from == message.From && _to == message.To)
        Set(message);
      else if (_from == message.From && _to == Zone.None)
        Set(message);
    }
  }
}