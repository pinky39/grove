namespace Grove.Effects
{
  using Modifiers;

  public class ExchangeForOpponentsCreature : Effect
  {
    protected override void ResolveEffect()
    {
      var targetModifier = new ChangeController(Controller);
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      Target.Card().AddModifier(targetModifier, p);

      var sourceModifier = new ChangeController(Controller.Opponent);
      Source.OwningCard.AddModifier(sourceModifier, p);
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