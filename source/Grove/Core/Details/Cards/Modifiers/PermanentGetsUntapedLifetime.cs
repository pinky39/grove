namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class PermanentGetsUntapedLifetime : Lifetime,
    IReceive<PermanentGetsUntapped>, IReceive<CardChangedZone>
  {
    private readonly Card _permanent;

    private PermanentGetsUntapedLifetime() {}

    public PermanentGetsUntapedLifetime(Card permanent, ChangeTracker changeTracker)
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

    public void Receive(PermanentGetsUntapped message)
    {
      if (message.Permanent == _permanent)
      {
        End();
      }
    }
  }
}