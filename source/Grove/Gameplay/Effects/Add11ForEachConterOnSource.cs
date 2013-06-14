namespace Grove.Gameplay.Effects
{
  using Modifiers;

  public class Add11ForEachConterOnSource : Effect
  {
    private int _countersCount;

    protected override void Initialize()
    {
      _countersCount = Source.OwningCard.Counters;
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