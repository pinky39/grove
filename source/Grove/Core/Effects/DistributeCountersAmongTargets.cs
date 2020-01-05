namespace Grove.Effects
{
  using System;  
  using Modifiers;

  public class DistributeCountersAmongTargets : Effect
  {
    private readonly Func<Counter> _createCounter;      

    private DistributeCountersAmongTargets() {}

    public DistributeCountersAmongTargets(Func<Counter> createCounter)  
    {
      _createCounter = createCounter;      
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };
      
      for (var i = 0; i < Targets.Effect.Count; i++)
      {
        var target = Targets.Effect[i];
        var count = Targets.Distribution[i];

        if (IsValid(target) && count > 0)
        {
          var modifier = new AddCounters(_createCounter, count);
          target.Card().AddModifier(modifier, p);
        }                
      }        
    }
  }
}
