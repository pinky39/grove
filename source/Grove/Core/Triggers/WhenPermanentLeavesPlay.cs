namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class WhenPermanentLeavesPlay : Trigger, IReceive<ZoneChangedEvent>
  {
    private readonly Card _permanent;

    private WhenPermanentLeavesPlay() {}

    public WhenPermanentLeavesPlay(Card permanent)
    {
      _permanent = permanent;
    }

    public void Receive(ZoneChangedEvent e)
    {
      if (e.FromBattlefield && e.Card == _permanent)
      {
        Set(e);
      }
    }
  }
}