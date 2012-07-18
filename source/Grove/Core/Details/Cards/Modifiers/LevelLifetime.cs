namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class LevelLifetime : Lifetime, IReceive<CardChangedLevel>
  {
    private readonly int? _maxLevel;
    private readonly int _minLevel;
    private readonly Card _modifierTarget;
    private LevelLifetime() {}

    public LevelLifetime(int minLevel, int? maxLevel, Card modifierTarget,
      ChangeTracker changeTracker) : base(changeTracker)
    {
      _minLevel = minLevel;
      _maxLevel = maxLevel;
      _modifierTarget = modifierTarget;
    }


    public void Receive(CardChangedLevel message)
    {
      if (message.Card != _modifierTarget)
        return;

      if (_modifierTarget.Level < _minLevel ||
        _modifierTarget.Level > _maxLevel)
      {
        End();
      }
    }
  }
}