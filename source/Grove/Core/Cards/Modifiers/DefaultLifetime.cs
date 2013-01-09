namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;
  using Grove.Core.Targeting;

  public class DefaultLifetime : Lifetime, IReceive<ZoneChanged>
  {
    public ITarget Target { get; set; }
        
    public void Receive(ZoneChanged message)
    {
      if (message.Card != Target)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}