namespace Grove.Core.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class TimingRuleParameters
  {
    private readonly Targets _targets;
    public Card Card { get; private set; }
    public Player Controller {get { return Card.Controller; }}
    
    public Targets Targets()
    {
      return _targets;
    }
    
    public IEnumerable<T> Targets<T>()
    {
      return _targets.Where(x => x is T).Select(x => (T) x);
    }
    
    public TimingRuleParameters(Card card, Targets targets = null)
    {
      Card = card;
      _targets = targets;
    }
  }
}