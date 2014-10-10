namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnPlayerSearchesLibrary : Trigger, IReceive<PlayerSearchesLibrary>
  {
    private readonly Func<TriggeredAbility, Player, bool> _filter;

    private OnPlayerSearchesLibrary() {}

    public OnPlayerSearchesLibrary(Func<TriggeredAbility, Player, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(PlayerSearchesLibrary message)
    {
      if (_filter(Ability, message.Player))
      {
        Set(message);
      }
    }
  }
}
