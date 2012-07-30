namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class PermanentLeavesBattlefieldLifetime : Lifetime, IReceive<CardChangedZone>
  {
    private readonly Card _permanent;

    private PermanentLeavesBattlefieldLifetime()
    {      
    }
    
    public PermanentLeavesBattlefieldLifetime(Card permanent, ChangeTracker changeTracker)
      : base(changeTracker)
    {
      _permanent = permanent;
    }
    
    public void Receive(CardChangedZone message)
    {
      if (message.Card == _permanent && message.FromBattlefield)
      {
        End();
      }
    }
  }
}