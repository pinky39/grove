namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnOwnerGetsTapped : Trigger, IReceive<PermanentTappedEvent>
  {
    public void Receive(PermanentTappedEvent message)
    {
      if (message.Card != OwningCard)
        return;

      Set(message);
    }
  }
}