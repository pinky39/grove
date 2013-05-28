namespace Grove.Gameplay.Triggers
{
  using System;
  using Abilities;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class OnCastedSpell : Trigger, IReceive<PlayerHasCastASpell>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnCastedSpell() {}

    public OnCastedSpell(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(PlayerHasCastASpell message)
    {
      if (!_filter(Ability, message.Card))
        return;

      Set(message);
    }
  }
}