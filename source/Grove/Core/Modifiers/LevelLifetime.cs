namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class LevelLifetime : Lifetime, IReceive<LevelChanged>
  {
    private readonly int? _maxLevel;
    private readonly int _minLevel;
    private readonly Card _modifierTarget;

    public LevelLifetime(int minLevel, int? maxLevel, Card modifierTarget)
    {
      _minLevel = minLevel;
      _maxLevel = maxLevel;
      _modifierTarget = modifierTarget;
    }

    public void Receive(LevelChanged message)
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