namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class WhenPlayerDiscardsCard : Trigger, IReceive<PlayerDiscardsCardEvent>
  {
    private readonly TriggerPredicate<PlayerDiscardsCardEvent> _cond;

    private WhenPlayerDiscardsCard() {}

    public WhenPlayerDiscardsCard(TriggerPredicate<PlayerDiscardsCardEvent> cond = null)
    {
      _cond = cond ?? delegate { return true; };
    }

    public void Receive(PlayerDiscardsCardEvent message)
    {
      if (_cond(message, Ctx))
      {
        Set(message);
      }
    }
  }
}