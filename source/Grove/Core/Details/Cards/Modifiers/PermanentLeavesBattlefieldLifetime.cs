namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class PermanentLeavesBattlefieldLifetime : Lifetime, IReceive<CardChangedZone>
  {
    public Card Permanent { get; set; }
    
    public void Receive(CardChangedZone message)
    {
      if (message.Card == Permanent && message.FromBattlefield)
      {
        End();
      }
    }
  }
}