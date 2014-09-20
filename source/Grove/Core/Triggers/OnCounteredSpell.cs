namespace Grove.Triggers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnCounteredSpell : Trigger, IReceive<SpellCounteredEvent>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnCounteredSpell() {}

    public OnCounteredSpell(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(SpellCounteredEvent message)
    {
      if (_filter(Ability, message.Card))
      {
        Set(message);
      }
    }
  }
}