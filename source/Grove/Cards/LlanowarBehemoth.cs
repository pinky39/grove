namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class LlanowarBehemoth : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
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
          C.ActivatedAbility(
            "Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.",
            C.Cost<TapCreature>(),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                }, untilEndOfTurn: true))),
            
            costValidator: C.Validator(Validators.Creature((creature) => !creature.IsTapped, Controller.SpellOwner), 
              mustBeTargetable: false),
            
            aiTargetFilter: AiTargetSelectors.CostTap(),
            category: EffectCategories.ToughnessIncrease,
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 1)
            ));
    }
  }
}