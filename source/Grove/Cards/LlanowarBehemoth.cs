namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class LlanowarBehemoth : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Llanowar Behemoth")
        .ManaCost("{3}{G}{G}")
        .Type("Creature - Elemental")
        .Text("Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.")
        .FlavorText(
          "'Most people can't build decent weapons out of stone or steel. Trust the elves to do it with only mud and vines.'{EOL}—Gerrard of the Weatherlight")
        .Power(4)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.",
            Cost<TapCreature>(),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                }, untilEndOfTurn: true))),
            
            costValidator: TargetValidator(TargetIs.Creature((creature) => !creature.IsTapped, Controller.SpellOwner), 
              mustBeTargetable: false),
            
            targetSelectorAi: TargetSelectorAi.CostTapOrSacCreature(),
            category: EffectCategories.ToughnessIncrease,
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 1)
            ));
    }
  }
}