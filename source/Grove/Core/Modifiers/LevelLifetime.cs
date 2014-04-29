namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class LevelLifetime : Lifetime, IReceive<LevelChangedEvent>
  {
    private readonly int? _maxLevel;
    private readonly int _minLevel;

    private LevelLifetime() {}

    public LevelLifetime(int minLevel, int? maxLevel)
    {
      _minLevel = minLevel;
      _maxLevel = maxLevel;
    }

    public void Receive(LevelChangedEvent message)
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