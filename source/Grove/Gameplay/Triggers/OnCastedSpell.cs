namespace Grove.Gameplay.Triggers
{
  using System;
  using Infrastructure;
  using Messages;

  public class OnCastedSpell : Trigger, IReceive<AfterSpellWasPutOnStack>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnCastedSpell() {}

    public OnCastedSpell(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(AfterSpellWasPutOnStack message)
    {
      if (_filter(Ability, message.Card))
      {
        Set(message);
      }
    }
  }
}