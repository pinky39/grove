namespace Grove.Effects
{
  using System.Collections.Generic;
  using AI;
  using Modifiers;

  public class PutDifferentAmountOfCountersOnTargets : Effect
  {
    private readonly int _power;
    private readonly int _toughness;
    private readonly List<int> _amounts = new List<int>();

    private PutDifferentAmountOfCountersOnTargets() {}

    public PutDifferentAmountOfCountersOnTargets(int power, int toughness, IEnumerable<int> amounts)
    {
      _power = power;
      _toughness = toughness;
      _amounts.AddRange(amounts);

      SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };

      for (var i = 0; i < _amounts.Count; i++)
      {
        var target = Targets.Effect[i];

        if (IsValid(target))
        {
          target.Card().AddModifier(new AddCounters(() => new PowerToughness(_power, _toughness), _amounts[i]), p);
        }
      }    
    }
  }
}
