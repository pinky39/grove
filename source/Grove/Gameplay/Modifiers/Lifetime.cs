namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Misc;

  public abstract class Lifetime : GameObject
  {
    protected Lifetime()
    {
      Ended = new TrackableEvent(this);
    }

    public TrackableEvent Ended { get; set; }
    public Modifier Modifier { get; private set; }
    protected Card OwningCard { get { return Modifier.OwningCard; } }


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