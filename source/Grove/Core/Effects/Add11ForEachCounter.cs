namespace Grove.Effects
{
  using Grove.AI;
  using Modifiers;

  public class Add11ForEachCounter : Effect
  {
    private int _countersCount;

    protected override void Initialize()
    {
      _countersCount = Source.OwningCard.Counters;
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

      var modifier = new AddPowerAndToughness(_countersCount, _countersCount) {UntilEot = true};

      var targetCreature = (Card) Target;
      targetCreature.AddModifier(modifier, p);
    }
  }
}