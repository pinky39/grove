namespace Grove.Core.Cards.Effects
{
  using Modifiers;
  using Targeting;

  public class ExchangeForOpponentsCreature : Effect
  {
    protected override void ResolveEffect()
    {           
      var targetModifier = Builder
        .Modifier<ChangeController>(m => m.NewController = Controller)
        .CreateModifier(Source.OwningCard, Source.OwningCard, X, Game);

      Target().Card().AddModifier(targetModifier);

      var opponent = Game.Players.GetOpponent(Controller);
      
      var sourceModifier = Builder
        .Modifier<ChangeController>(m => m.NewController = opponent)
        .CreateModifier(Source.OwningCard, Source.OwningCard, X, Game);

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