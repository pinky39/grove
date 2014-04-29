namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class ModifierSourceGetsUntapedLifetime : Lifetime,
    IReceive<PermanentUntappedEvent>, IReceive<ZoneChangedEvent>
  {
    public void Receive(PermanentUntappedEvent message)
    {
      if (message.Card == Modifier.SourceCard)
      {
        End();
      }
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.Card == Modifier.SourceCard && message.FromBattlefield)
      {
        End();
      }
    }
  }
}