namespace Grove.Triggers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnLandPlayed : Trigger, IReceive<PlayerPlayedALand>
  {
    private readonly Func<TriggeredAbility, Card, bool> _filter;

    private OnLandPlayed() {}

    public OnLandPlayed(Func<TriggeredAbility, Card, bool> filter = null)
    {
      _filter = filter;
    }

    public void Receive(PlayerPlayedALand message)
    {
      if (_filter(Ability, message.Card))
      {
        Set(message);
      }
    }
  }
}