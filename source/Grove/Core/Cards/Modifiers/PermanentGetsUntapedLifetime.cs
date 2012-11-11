namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

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