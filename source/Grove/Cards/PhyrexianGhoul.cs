namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
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
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.",
            Cost<SacPermanent>(),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }, untilEndOfTurn: true)
              )),
            costValidator: Validator(Validators.Creature(controller: Controller.SpellOwner), mustBeTargetable: false),
            selectorAi: TargetSelectorAi.CostTapOrSacCreature(canUseSelf: false),
            timing: All(Timings.IncreaseOwnersPowerAndThougness(2, 2)),
            category: EffectCategories.ToughnessIncrease
            )
        );
    }
  }
}