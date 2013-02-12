namespace Grove.Core.Effects
{
  using Modifiers;
  using Targeting;

  public class ExchangeForOpponentsCreature : Effect
  {
    protected override void ResolveEffect()
    {
      var targetModifier = new ChangeController(Controller);
      targetModifier.Initialize(new ModifierParameters
        {
          Source = Source.OwningCard,
          Target = Target,
          X = X
        }, Game);

      Target.Card().AddModifier(targetModifier);

      var sourceModifier = new ChangeController(Controller.Opponent);
      sourceModifier.Initialize(new ModifierParameters
        {
          Source = Source.OwningCard,
          Target = Source.OwningCard,
          X = X
        }, Game);

      Source.OwningCard.AddModifier(sourceModifier);
    }

    protected override void OnEffectCountered(SpellCounterReason reason)
    {
      if (reason == SpellCounterReason.IllegalTarget)
      {
        Source.SourceCard.Sacrifice();
      }
    }
  }
}