namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

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