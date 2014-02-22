namespace Grove.Gameplay.AI.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  public class TimingRuleParameters
  {
    private readonly Targets _targets;

    public TimingRuleParameters(Card card, Targets targets = null, int? x = null)
    {
      Card = card;
      X = x;
      _targets = targets;
    }

    public Card Card { get; private set; }
    public int? X { get; private set; }
    public Player Controller { get { return Card.Controller; } }

    public Targets Targets()
    {
      return _targets;
    }

    public IEnumerable<T> Targets<T>()
    {
      Asrt.True(_targets != null,
        "This timing rule requires the targeting rule to be applied first.");

      return _targets.Where(x => x is T).Select(x => (T) x);
    }
  }
}