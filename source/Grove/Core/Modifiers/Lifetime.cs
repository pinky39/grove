namespace Grove.Modifiers
{
  using Infrastructure;

  public abstract class Lifetime : GameObject
  {
    public TrackableEvent Ended = new TrackableEvent();
    public Modifier Modifier { get; private set; }

    protected Card OwningCard
    {
      get { return Modifier.OwningCard; }
    }

    protected void End()
    {
      Ended.Raise();
    }

    public virtual void Initialize(Game game, Modifier modifier = null)
    {
      Game = game;
      Modifier = modifier;
      Ended.Initialize(game.ChangeTracker);
    }
  }
}