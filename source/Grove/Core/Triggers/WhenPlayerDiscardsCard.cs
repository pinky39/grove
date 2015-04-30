namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenPlayerDiscardsCard : Trigger, IReceive<PlayerDiscardsCardEvent>
  {
    private readonly Func<TriggeredAbility, Player, Card, bool> _filter;

    private WhenPlayerDiscardsCard() {}

    public WhenPlayerDiscardsCard(Func<TriggeredAbility, Player, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(PlayerDiscardsCardEvent message)
    {
      if (_filter(Ability, message.Player, message.Card))
      {
        Set(message);
      }
    }
  }
}
