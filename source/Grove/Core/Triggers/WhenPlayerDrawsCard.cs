namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenPlayerDrawsCard : Trigger, IReceive<PlayerDrawsCardEvent>
  {
    private readonly Func<TriggeredAbility, Player, bool> _filter;

    private WhenPlayerDrawsCard() {}

    public WhenPlayerDrawsCard(Func<TriggeredAbility, Player, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(PlayerDrawsCardEvent message)
    {
      if (_filter(Ability, message.Player))
      {
        Set(message);
      }
    }
  }
}
