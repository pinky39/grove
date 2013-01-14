namespace Grove.Core.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class PermanentLeavesBattlefieldLifetime : Lifetime, IReceive<ZoneChanged>
  {
    public Card Permanent { get; set; }
    
    public void Receive(ZoneChanged message)
    {
      if (message.Card == Permanent && message.FromBattlefield)
      {
        End();
      }
    }
  }
}