namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;

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