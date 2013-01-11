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
        .Abilities(
          ActivatedAbility(
            "Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.",
            Cost<Tap>(),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                }, untilEndOfTurn: true))),
            
            costTarget: Target(
              Validators.Card(ControlledBy.SpellOwner, card => !card.IsTapped && card.Is().Creature), 
              Zones.Battlefield(),
              mustBeTargetable: false),
            
            targetingAi: TargetingAi.CostTapOrSacCreature(),
            category: EffectCategories.ToughnessIncrease,
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 1)
            ));
    }
  }
}