namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnCounteredSpell : Trigger, IReceive<SpellWasCountered>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnCounteredSpell() {}

    public OnCounteredSpell(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(SpellWasCountered message)
    {
      if (_filter(Ability, message.Card))
      {
        Set(message);
      }
    }
  }
}