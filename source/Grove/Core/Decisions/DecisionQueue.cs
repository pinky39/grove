namespace Grove.Core.Decisions
{
  using Grove.Infrastructure;

  [Copyable]
  public class DecisionQueue
  {
    private readonly TrackableList<IDecision> _queue;

    private DecisionQueue() {}

    public DecisionQueue(ChangeTracker changeTracker)
    {
      _queue = new TrackableList<IDecision>(
        orderImpactsHashcode: true).Initialize(changeTracker);
    }

    public int Count { get { return _queue.Count; } }

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