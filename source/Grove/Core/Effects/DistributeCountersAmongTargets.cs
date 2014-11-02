namespace Grove.Effects
{
  using System.Linq;
  using Modifiers;

  public class DistributeCountersAmongTargets : Effect
  {
    private readonly AddCounters _modifier;
    private readonly int _counterAmount;

    private DistributeCountersAmongTargets() {}

    public DistributeCountersAmongTargets(AddCounters counter, int counterAmount)
    {
      _modifier = counter;
      _counterAmount = counterAmount;
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };

      if (_counterAmount < ValidEffectTargets.Count())
      {
        // Apply counter to each target
        foreach (var target in ValidEffectTargets)
        {
          target.Card().AddModifier(_modifier, p);
        }
      }
      else
      {
        // Apply all counters to one target
        for (int i = 0; i < _counterAmount; i++)
        {
          Target.Card().AddModifier(_modifier, p);
        }
      }

      // TODO: If one counter to one target and rest counters to another target?
    }
  }
}
