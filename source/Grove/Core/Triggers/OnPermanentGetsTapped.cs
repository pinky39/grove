namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnPermanentGetsTapped : Trigger, IReceive<PermanentTappedEvent>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnPermanentGetsTapped() {}

    public OnPermanentGetsTapped(Func<TriggeredAbility, Card, bool> filter)
    {
      _filter = filter;
    }

    public void Receive(PermanentTappedEvent message)
    {
      if (!_filter(Ability, message.Card))
        return;

      Set(message);
    }
  }
}