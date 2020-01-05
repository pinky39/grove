namespace Grove.Effects
{
  using System.Collections.Generic;
  using AI;
  using Modifiers;

  public class PutDifferentAmountOf11ContersOnTargets : Effect
  {    
    private readonly List<int> _amounts = new List<int>();

    private PutDifferentAmountOf11ContersOnTargets() {}

    public PutDifferentAmountOf11ContersOnTargets(IEnumerable<int> amounts)
    {      
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
          target.Card().AddModifier(new AddCounters(() => new PowerToughness(1, 1), _amounts[i]), p);
        }
      }    
    }
  }
}
