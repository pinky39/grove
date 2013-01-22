namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class PhyrexianGhoul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Ghoul")
        .ManaCost("{2}{B}")
        .Type("Creature - Zombie")
        .Text("Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.")
        .FlavorText("Phyrexia wastes nothing. Its food chain is a spiraling cycle.")
        .Power(2)
        .Toughness(2)        
        .Abilities(
          ActivatedAbility(
            "Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.",
            Cost<Sacrifice>(),
            Effect<Core.Effects.ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }, untilEndOfTurn: true)
              )),
            costTarget: Target(
              Validators.Card(ControlledBy.SpellOwner, card => card.Is().Creature),
              Zones.Battlefield(), mustBeTargetable: false),
            targetingAi: TargetingAi.CostTapOrSacCreature(preferSelf: false),
            timing: All(Timings.IncreaseOwnersPowerAndThougness(2, 2)),
            category: EffectCategories.ToughnessIncrease
            )
        );
    }
  }
}