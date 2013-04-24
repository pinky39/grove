namespace Grove.Ai.TimingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class TimingRuleParameters
  {
    private readonly Targets _targets;
    public Card Card { get; private set; }
    public int? X { get; private set; }
    public Player Controller {get { return Card.Controller; }}
    
    public Targets Targets()
    {
      return _targets;
    }
    
    public IEnumerable<T> Targets<T>()
    {
      if (_targets == null)
        throw new InvalidOperationException("This timing rule requires the targeting rule to be applied first.");
      
      return _targets.Where(x => x is T).Select(x => (T) x);
    }
    
    public TimingRuleParameters(Card card, Targets targets = null, int? x = null)
    {
      Card = card;
      X = x;
      _targets = targets;
    }
  }
}