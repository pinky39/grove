namespace Grove.Triggers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnOwnerGetsTapped : Trigger, IReceive<PermanentGetsTapped>
  {
    public void Receive(PermanentGetsTapped message)
    {
      if (message.Permanent != OwningCard)
        return;

      Set(message);
    }
  }
}