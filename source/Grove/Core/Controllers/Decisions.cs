namespace Grove.Core.Controllers
{
  using System;
  using Infrastructure;

  [Copyable]
  public class Decisions
  {
    private readonly DecisionFactory _decisionFactory;
    private readonly DecisionQueue _decisionQueue;

    private Decisions() {}

    public Decisions(DecisionQueue decisionQueue, DecisionFactory decisionFactory)
    {
      _decisionQueue = decisionQueue;
      _decisionFactory = decisionFactory;
    }

    public void Init(Game game)
    {
      _decisionFactory.Init(game);
    }

    public void Enqueue<TDecision>(Player controller, Action<TDecision> init = null)
      where TDecision : IDecision
    {
      init = init ?? delegate { };

      var decision = _decisionFactory.Create(controller, init);
      _decisionQueue.Enqueue(decision);
    }
  }
}