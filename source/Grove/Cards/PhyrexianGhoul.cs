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

  public class PhyrexianGhoul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Phyrexian Ghoul")
        .ManaCost("{2}{B}")
        .Type("Creature - Zombie")
        .Text("Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.")
        .FlavorText("Phyrexia wastes nothing. Its food chain is a spiraling cycle.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.",
            C.Cost<SacrificeCreature>(),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                })
              )),
            costValidator: C.Validator(Validators.Creature(controller: Controller.SpellOwner)),
            targetSelectorAi: TargetSelectorAi.CostTapOrSacCreature(canUseSelf: false),
            timing: All(Timings.IncreaseOwnersPowerAndThougness(2, 2)),
            category: EffectCategories.ToughnessIncrease
            )
        );
    }
  }
}