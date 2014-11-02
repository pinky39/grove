namespace Grove.Effects
{
  using Modifiers;

  public class PutIncrementalCountersOnTargets : Effect
  {
    private readonly int _power;
    private readonly int _toughness;

    private PutIncrementalCountersOnTargets() {}

    public PutIncrementalCountersOnTargets(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
      {
        SourceEffect = this,
        SourceCard = Source.OwningCard,
        X = X
      };

      int count = 1;

      foreach (var target in ValidEffectTargets)
      {
        target.Card().AddModifier(new AddCounters(() => new PowerToughness(_power, _toughness), count), p);

        count++;
      }      
    }
  }
}
