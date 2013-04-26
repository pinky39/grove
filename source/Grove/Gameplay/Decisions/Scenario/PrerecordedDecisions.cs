namespace Grove.Gameplay.Decisions.Scenario
{
  using System.Collections.Generic;
  using System.Linq;
  using States;

  public class PrerecordedDecisions
  {
    private readonly List<DecisionsForOneStep> _decisions = new List<DecisionsForOneStep>();

    public void AddDecisions(IEnumerable<DecisionsForOneStep> decisions)
    {
      _decisions.AddRange(decisions);
    }

    public IDecision GetNext<TDecision>(int turn, Step step)
    {
      var decisions = _decisions
        .Where(x => x.Step == step && x.Turn == turn)
        .SingleOrDefault();

      if (decisions == null)
      {
        return null;
      }

      return decisions.Next<TDecision>();
    }
  }
}