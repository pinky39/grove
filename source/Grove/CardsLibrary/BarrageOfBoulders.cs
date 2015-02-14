namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class BarrageOfBoulders : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Barrage of Boulders")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text("Barrage of Boulders deals 1 damage to each creature you don't control.{EOL}{I}Ferocious{/I} — If you control a creature with power 4 or greater, creatures can't block this turn.")
        .FlavorText("Crude tactics can be effective nonetheless.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToCreaturesAndPlayers(
            amountCreature: (e, creature) => e.X.GetValueOrDefault(),
            filterCreature: (effect, card) => card.Has().Flying);
          p.Effect = () => new FerociousEffect(
            normalEffects: new Effect[]
            {
              new DealDamageToCreaturesAndPlayers(
                amountCreature: 1,
                filterCreature: (effect, card) => card.Controller != effect.Source.OwningCard.Controller),
            },
            ferociousEffects: new Effect[]
            {
              new DealDamageToCreaturesAndPlayers(
                amountCreature: 1,
                filterCreature: (effect, card) => card.Controller != effect.Source.OwningCard.Controller),
              new ApplyModifiersToPermanents(
                selector: (e, c) => c.Is().Creature,
                controlledBy: ControlledBy.Opponent,
                modifiers: () => new AddStaticAbility(Static.CannotBlock){UntilEot = true}), 
            });


          p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
