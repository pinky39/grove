namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class DefaultLifetime : Lifetime, IReceive<CardChangedZone>
  {
    public ITarget Target { get; set; }
        
    public void Receive(CardChangedZone message)
    {
      if (message.Card != Target)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}