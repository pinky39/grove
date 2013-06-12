namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

  public class LevelLifetime : Lifetime, IReceive<LevelChanged>
  {
    private readonly int? _maxLevel;
    private readonly int _minLevel;

    private LevelLifetime() {}

    public LevelLifetime(int minLevel, int? maxLevel)
    {
      _minLevel = minLevel;
      _maxLevel = maxLevel;
    }

    public void Receive(LevelChanged message)
    {
      if (message.Card != OwningCard)
        return;

      if (OwningCard.Level < _minLevel ||
        OwningCard.Level > _maxLevel)
      {
        End();
      }
    }
  }
}