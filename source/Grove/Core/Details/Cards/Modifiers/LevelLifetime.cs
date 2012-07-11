namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class LevelLifetime : Lifetime, IReceive<CardChangedLevel>
  {
    private LevelLifetime() {}
    public LevelLifetime(Modifier modifier, ChangeTracker changeTracker) : base(modifier, changeTracker) {}

    public int MinLevel { get; set; }
    public int? MaxLevel { get; set; }

    public void Receive(CardChangedLevel message)
    {
      if (message.Card != ModifierTarget)
        return;

      if (ModifierTarget.Card().Level < MinLevel || ModifierTarget.Card().Level > MaxLevel)
      {
        End();
      }
    }
  }
}