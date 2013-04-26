namespace Grove.Gameplay.Decisions
{
  using Infrastructure;

  [Copyable]
  public class DecisionQueue
  {
    private readonly TrackableList<IDecision> _queue = new TrackableList<IDecision>(orderImpactsHashcode: true);

    public int Count { get { return _queue.Count; } }

    public void Initialize(Game game)
    {
      _queue.Initialize(game.ChangeTracker);
    }

    public IDecision Dequeue()
    {
      var next = _queue[0];
      _queue.Remove(next);
      return next;
    }

    public void Enqueue(IDecision decision)
    {
      _queue.Add(decision);
    }
  }
}