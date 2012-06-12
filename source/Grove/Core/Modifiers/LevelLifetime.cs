namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class LevelLifetime : Lifetime, IReceive<CardChangedLevel>
  {
    private LevelLifetime() { }

    public int MinLevel { get; set; }
    public int? MaxLevel { get; set; }

    public LevelLifetime(Modifier modifier, ChangeTracker changeTracker) : base(modifier, changeTracker) { }
    
    public void Receive(CardChangedLevel message)
    {            
      if (message.Card != ModifierTarget)
        return;
      
      if (ModifierTarget.Level < MinLevel || ModifierTarget.Level > MaxLevel)
      {
        End();
      }
    }
  }
}