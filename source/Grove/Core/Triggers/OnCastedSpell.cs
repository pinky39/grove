namespace Grove.Triggers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnCastedSpell : Trigger, IReceive<SpellPutOnStackEvent>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnCastedSpell() {}

    public OnCastedSpell(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(SpellPutOnStackEvent message)
    {
      if (_filter(Ability, message.Card))
      {
        Set(message);
      }
    }
  }
}