namespace Grove.Gameplay.Decisions
{
  using Infrastructure;

  [Copyable]
  public class DecisionQueue : IHashable
  {
    private readonly TrackableList<Decision> _queue = new TrackableList<Decision>(orderImpactsHashcode: true);

    public int Count { get { return _queue.Count; } }

    public int CalculateHash(HashCalculator calc)
    {
      return _queue.Count;
    }

    public void Initialize(Game game)
    {
      _queue.Initialize(game.ChangeTracker);
    }

    public Decision Dequeue()
    {
      var next = _queue[0];
      _queue.Remove(next);
      return next;
    }

    public void Enqueue(Decision decision)
    {
      _queue.Add(decision);
    }
  }
}