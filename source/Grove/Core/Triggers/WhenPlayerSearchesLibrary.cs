namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class WhenPlayerSearchesLibrary : Trigger, IReceive<PlayerSearchesLibrary>
  {
    private readonly TriggerPredicate<Player> _cond;

    private WhenPlayerSearchesLibrary() {}

    public WhenPlayerSearchesLibrary(TriggerPredicate<Player> cond = null)
    {
      _cond = cond ?? delegate { return true; };
    }

    public void Receive(PlayerSearchesLibrary message)
    {
      if (_cond(message.Player, Ctx))
      {
        Set(message);
      }
    }
  }
}