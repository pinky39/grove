namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class PermanentGetsUntapedLifetime : Lifetime,
    IReceive<PermanentGetsUntapped>, IReceive<CardChangedZone>
  {
    public Card Permanent { get; set; }

    public void Receive(CardChangedZone message)
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