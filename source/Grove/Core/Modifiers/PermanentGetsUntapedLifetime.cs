namespace Grove.Core.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class PermanentGetsUntapedLifetime : Lifetime,
    IReceive<PermanentGetsUntapped>, IReceive<ZoneChanged>
  {
    public Card Permanent { get; set; }

    public void Receive(ZoneChanged message)
    {
      if (message.Card == Permanent && message.FromBattlefield)
      {
        End();
      }
    }

    public void Receive(PermanentGetsUntapped message)
    {
      if (message.Permanent == Permanent)
      {
        End();
      }
    }
  }
}