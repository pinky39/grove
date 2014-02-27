namespace Grove.Triggers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnPermanentGetsTapped : Trigger, IReceive<PermanentGetsTapped>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnPermanentGetsTapped() {}

    public OnPermanentGetsTapped(Func<TriggeredAbility, Card, bool> filter)
    {
      _filter = filter;
    }

    public void Receive(PermanentGetsTapped message)
    {
      if (!_filter(Ability, message.Permanent))
        return;

      Set(message);
    }
  }
}