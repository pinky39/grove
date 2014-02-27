namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class OwningCardLifetime : Lifetime, IReceive<ZoneChanged>
  {
    public void Receive(ZoneChanged message)
    {
      if (message.Card != Modifier.Owner)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}