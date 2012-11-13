namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;
  using Grove.Core.Targeting;

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