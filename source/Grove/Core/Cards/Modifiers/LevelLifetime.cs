namespace Grove.Core.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class LevelLifetime : Lifetime, IReceive<LevelChanged>
  {
    public int? MaxLevel { get; set; }
    public int MinLevel { get; set; }
    public Card ModifierTarget { get; set; }

    public void Receive(LevelChanged message)
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