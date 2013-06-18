namespace Grove.Gameplay.Triggers
{
  using System;
  using Abilities;
  using Infrastructure;
  using Messages;

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