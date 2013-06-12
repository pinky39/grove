namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

  public class ModifierSourceGetsUntapedLifetime : Lifetime,
    IReceive<PermanentGetsUntapped>, IReceive<ZoneChanged>
  {
    public void Receive(PermanentGetsUntapped message)
    {
      if (message.Permanent == Modifier.SourceCard)
      {
        End();
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (message.Card == Modifier.SourceCard && message.FromBattlefield)
      {
        End();
      }
    }
  }
}