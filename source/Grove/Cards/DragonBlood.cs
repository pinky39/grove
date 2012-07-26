namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Counters;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class DragonBlood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Dragon Blood")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3},{T} : Put a +1/+1 counter on target creature.")
        .FlavorText("Fire in the blood, fire in the belly.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.ActivatedAbility(
            "{3},{T} : Put a +1/+1 counter on target creature.",
            C.Cost<TapOwnerPayMana>((cost, _) =>
              {
                cost.TapOwner = true;
                cost.Amount = 3.AsColorlessMana();
              }),
            C.Effect<ApplyModifiersToTarget>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddCounters>((m, c) => m.Counter =
                c.Counter<PowerToughness>((ct, _) =>
                  {
                    ct.Power = 1;
                    ct.Toughness = 1;
                  })
                ))),
            effectValidator: C.Validator(Validators.Creature()),
            aiTargetFilter: AiTargetSelectors.IncreasePowerAndToughness(1, 1),
            timing: Timings.NoRestrictions()
            ));
    }
  }
}