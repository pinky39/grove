namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class DragonBlood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Dragon Blood")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3},{T} : Put a +1/+1 counter on target creature.")
        .FlavorText("Fire in the blood, fire in the belly.")
        .Timing(Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{3},{T} : Put a +1/+1 counter on target creature.",
            Cost<TapOwnerPayMana>(cost =>
              {
                cost.TapOwner = true;
                cost.Amount = 3.AsColorlessMana();
              }),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddCounters>(m => m.Counter =
                Counter<PowerToughness>(c =>
                  {
                    c.Power = 1;
                    c.Toughness = 1;
                  })
                ))),
            effectValidator: TargetValidator(TargetIs.Creature()),
            targetSelectorAi: TargetSelectorAi.IncreasePowerAndToughness(1, 1, untilEot: false),
            timing: Timings.NoRestrictions()
            ));
    }
  }
}