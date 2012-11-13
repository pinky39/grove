namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class LevelLifetime : Lifetime, IReceive<CardChangedLevel>
  {
    public int? MaxLevel { get; set; }
    public int MinLevel { get; set; }
    public Card ModifierTarget { get; set; }
        
    public void Receive(CardChangedLevel message)
    {
      if (message.Card != ModifierTarget)
        return;

      if (ModifierTarget.Level < MinLevel ||
        ModifierTarget.Level > MaxLevel)
      {
        End();
      }
    }
  }
}