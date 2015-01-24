namespace Grove.Effects
{
  using AI;
  using Modifiers;

  public class Put11CountersOnTargets : Effect
  {
    private readonly DynParam<int> _count;

    private Put11CountersOnTargets() {}

    public Put11CountersOnTargets(DynParam<int> count)
    {
      _count = count;

      RegisterDynamicParameters(_count);

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

      var modifier = new AddCounters(() => new PowerToughness(1, 1), count: _count.Value);

      var targetCreature = (Card)Target;
      targetCreature.AddModifier(modifier, p);
    }
  }
}
